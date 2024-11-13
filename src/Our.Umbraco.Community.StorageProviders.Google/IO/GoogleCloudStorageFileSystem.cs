using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Net;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace Our.Umbraco.Community.StorageProviders.Google.IO;
/// <inheritdoc />
public sealed class GoogleCloudStorageFileSystem : IGoogleCloudStorageFileSystem
{

    private readonly string _requestRootPath;
    private readonly string _bucketRootPath;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly GoogleCredential _credential;

    private readonly IIOHelper _ioHelper;
    private readonly IContentTypeProvider _contentTypeProvider;
    private readonly IOptionsMonitor<GoogleCloudStorageFileSystemOptions> _optionsMonitor;

    public GoogleCloudStorageFileSystem(GoogleCloudStorageFileSystemOptions options, GoogleCredential credential, IHostingEnvironment hostingEnvironment, IIOHelper ioHelper, IContentTypeProvider contentTypeProvider, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor)
        : this(GetRequestRootPath(options, hostingEnvironment), credential, options.BucketName, ioHelper, contentTypeProvider, optionsMonitor, null) //TODO FIX NULL
    { }

    public GoogleCloudStorageFileSystem(string requestRootPath, GoogleCredential credential, string bucketName, IIOHelper ioHelper, IContentTypeProvider contentTypeProvider, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, string? bucketRootPath = null)
    {
        ArgumentNullException.ThrowIfNull(requestRootPath);
        ArgumentNullException.ThrowIfNull(credential);
        ArgumentNullException.ThrowIfNull(bucketName);
        ArgumentNullException.ThrowIfNull(ioHelper);
        ArgumentNullException.ThrowIfNull(contentTypeProvider);

        _credential = credential;
        _requestRootPath = EnsureUrlSeparatorChar(requestRootPath).TrimEnd('/');
        _bucketRootPath = bucketRootPath ?? _requestRootPath;
        _storageClient = StorageClient.Create(credential);
        _bucketName = bucketName;
        _ioHelper = ioHelper;
        _contentTypeProvider = contentTypeProvider;
        _optionsMonitor = optionsMonitor;
    }
    public IEnumerable<string> GetDirectories(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteDirectory(string path, bool recursive)
    {
        throw new NotImplementedException();
    }

    public bool DirectoryExists(string path)
    {
        throw new NotImplementedException();
    }

    public void AddFile(string path, Stream stream)
    {
        throw new NotImplementedException();
    }

    public void AddFile(string path, Stream stream, bool overrideIfExists)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetFiles(string path)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetFiles(string path, string filter)
    {
        throw new NotImplementedException();
    }

    public Stream OpenFile(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(string path)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(string path)
    {
        throw new NotImplementedException();
    }

    public string GetRelativePath(string fullPathOrUrl)
    {
        throw new NotImplementedException();
    }

    public string GetFullPath(string path)
    {
        throw new NotImplementedException();
    }

    public string GetUrl(string path)
    {
        throw new NotImplementedException();
    }

    public DateTimeOffset GetLastModified(string path)
    {
        throw new NotImplementedException();
    }

    public DateTimeOffset GetCreated(string path)
    {
        throw new NotImplementedException();
    }

    public long GetSize(string path)
    {
        throw new NotImplementedException();
    }

    public void AddFile(string path, string physicalPath, bool overrideIfExists = true, bool copy = false)
    {
        throw new NotImplementedException();
    }

    public bool CanAddPhysical { get; }
    public StorageClient GetStorageClient(string path)
    {
        throw new NotImplementedException();
    }

    private static string GetRequestRootPath(GoogleCloudStorageFileSystemOptions options, IHostingEnvironment hostingEnvironment)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(hostingEnvironment);

        return hostingEnvironment.ToAbsolute(options.VirtualPath);
    }

    private static string EnsureUrlSeparatorChar(string path)
        => path.Replace("\\", "/", StringComparison.InvariantCultureIgnoreCase);
}