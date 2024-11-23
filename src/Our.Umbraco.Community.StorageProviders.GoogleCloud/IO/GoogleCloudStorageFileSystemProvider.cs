using System.Collections.Concurrent;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;

/// <inheritdoc />
public sealed class GoogleCloudStorageFileSystemProvider : IGoogleCloudStorageFileSystemProvider
{
    private readonly ConcurrentDictionary<string, IGoogleCloudStorageFileSystem> _fileSystems = new();
    private readonly IOptionsMonitor<GoogleCloudStorageFileSystemOptions> _optionsMonitor;
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly IIOHelper _ioHelper;
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public GoogleCloudStorageFileSystemProvider(IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, IHostingEnvironment hostingEnvironment, IIOHelper ioHelper)
    {
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        _ioHelper = ioHelper ?? throw new ArgumentNullException(nameof(ioHelper));
        _fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        _optionsMonitor.OnChange((options, name) => _fileSystems.TryRemove(name ?? Options.DefaultName, out _));
    }

    public IGoogleCloudStorageFileSystem GetFileSystem(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        GoogleCloudStorageFileSystemOptions googleCloudStorageFileSystemOptions = _optionsMonitor.Get(name);

        GoogleCredential credential;
        using (var jsonStream = new FileStream(googleCloudStorageFileSystemOptions.CredentialPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(jsonStream);
        }

        return _fileSystems.GetOrAdd(name, name =>
        {
            GoogleCloudStorageFileSystemOptions options = googleCloudStorageFileSystemOptions;
            return new GoogleCloudStorageFileSystem(options, credential, _hostingEnvironment, _ioHelper, _fileExtensionContentTypeProvider, _optionsMonitor);
        });
    }
}