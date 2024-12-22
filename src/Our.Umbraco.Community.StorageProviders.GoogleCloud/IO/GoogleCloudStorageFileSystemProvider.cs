// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/IO/AzureBlobFileSystemProvider.cs
// Adapted and modified in accordance with the terms of the MIT License.

using System.Collections.Concurrent;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.Services;
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
    private readonly GoogleCloudStorageService _googleCloudStorageService;

    public GoogleCloudStorageFileSystemProvider(IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, IHostingEnvironment hostingEnvironment, IIOHelper ioHelper, GoogleCloudStorageService googleCloudStorageService)
    {
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        _ioHelper = ioHelper ?? throw new ArgumentNullException(nameof(ioHelper));
        _googleCloudStorageService = googleCloudStorageService;
        _fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        _optionsMonitor.OnChange((options, name) => _fileSystems.TryRemove(name ?? Options.DefaultName, out _));
    }

    public IGoogleCloudStorageFileSystem GetFileSystem(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var googleCloudStorageFileSystemOptions = _optionsMonitor.Get(name);
        var storageClient = _googleCloudStorageService.GetStorageClient();

        return _fileSystems.GetOrAdd(name, name => 
            new GoogleCloudStorageFileSystem(
                googleCloudStorageFileSystemOptions, 
                storageClient, 
                _hostingEnvironment, 
                _ioHelper, _fileExtensionContentTypeProvider, _optionsMonitor));
    }
}