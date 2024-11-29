// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/IO/AzureBlobFileSystem.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;

/// <inheritdoc />
public sealed class GoogleCloudStorageFileSystem : IGoogleCloudStorageFileSystem, IFileProviderFactory
{
    private readonly string _requestRootPath;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly GoogleCredential _credential;

    private readonly IIOHelper _ioHelper;
    private readonly IOptionsMonitor<GoogleCloudStorageFileSystemOptions> _optionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleCloudStorageFileSystem"/> class.
    /// </summary>
    /// <param name="options">The Azure Blob File System options.</param>
    /// <param name="credential">The Google credential used for authenticating requests to Google Cloud Storage.</param>
    /// <param name="hostingEnvironment">The hosting environment.</param>
    /// <param name="ioHelper">The I/O helper.</param>
    /// <param name="contentTypeProvider">The content type provider.</param>
    /// <param name="optionsMonitor">A monitor to track changes to the <see cref="GoogleCloudStorageFileSystemOptions"/> configuration at runtime.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="options" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="hostingEnvironment" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="ioHelper" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="contentTypeProvider" /> is <c>null</c>.</exception>
    public GoogleCloudStorageFileSystem(GoogleCloudStorageFileSystemOptions options, GoogleCredential credential, IHostingEnvironment hostingEnvironment, IIOHelper ioHelper, IContentTypeProvider contentTypeProvider, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor)
        : this(GetRequestRootPath(options, hostingEnvironment), credential, options.BucketName, ioHelper, contentTypeProvider, optionsMonitor, null) //TODO FIX NULL
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleCloudStorageFileSystem"/> class.
    /// </summary>
    /// <param name="requestRootPath">The request/URL root path.</param>
    /// <param name="credential">The Google credential used for authenticating requests to Google Cloud Storage.</param>
    /// <param name="ioHelper">The I/O helper.</param>
    /// <param name="contentTypeProvider">The content type provider.</param>
    /// <param name="optionsMonitor">A monitor to track changes to the <see cref="GoogleCloudStorageFileSystemOptions"/> configuration at runtime.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="requestRootPath" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="ioHelper" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="contentTypeProvider" /> is <c>null</c>.</exception>
    public GoogleCloudStorageFileSystem(string requestRootPath, GoogleCredential credential, string bucketName, IIOHelper ioHelper, IContentTypeProvider contentTypeProvider, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, string? bucketRootPath = null)
    {
        ArgumentNullException.ThrowIfNull(requestRootPath);
        ArgumentNullException.ThrowIfNull(credential);
        ArgumentNullException.ThrowIfNull(bucketName);
        ArgumentNullException.ThrowIfNull(ioHelper);
        ArgumentNullException.ThrowIfNull(contentTypeProvider);

        _credential = credential;
        _requestRootPath = EnsureUrlSeparatorChar(requestRootPath).TrimEnd('/');
        _storageClient = StorageClient.Create(credential);
        _bucketName = bucketName;
        _ioHelper = ioHelper;
        _optionsMonitor = optionsMonitor;
    }
    /// <inheritdoc />
    public bool CanAddPhysical => false;


    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public IEnumerable<string> GetDirectories(string path)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public void DeleteDirectory(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        DeleteDirectory(path, true);
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public void DeleteDirectory(string path, bool recursive)
    {
        ArgumentNullException.ThrowIfNull(path);

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public bool DirectoryExists(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="stream" /> is <c>null</c>.</exception>
    public void AddFile(string path, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(stream);

        AddFile(path, stream, true);
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="stream" /> is <c>null</c>.</exception>
    public void AddFile(string path, Stream stream, bool overrideIfExists)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(stream);

        var objectExists = _storageClient.ListObjects(_bucketName, path).Any(o => o.Name == path);

        if (!overrideIfExists && objectExists)
        {
            throw new InvalidOperationException($"A file at path '{path}' already exists.");
        }

        var contentType = GetContentType(path);

        var uploadOptions = new UploadObjectOptions
        {
            PredefinedAcl = overrideIfExists ? null : PredefinedObjectAcl.BucketOwnerRead
        };

        _storageClient.UploadObject(_bucketName, path, contentType, stream, uploadOptions);

    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="physicalPath" /> is <c>null</c>.</exception>
    public void AddFile(string path, string physicalPath, bool overrideIfExists = true, bool copy = false)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(physicalPath);

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public IEnumerable<string> GetFiles(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        return GetFiles(path, null);
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public IEnumerable<string> GetFiles(string path, string? filter)
    {
        ArgumentNullException.ThrowIfNull(path);

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public Stream OpenFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        var memoryStream = new MemoryStream();
        _storageClient.DownloadObject(_bucketName, path, memoryStream);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;

    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public void DeleteFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        try
        {
            _storageClient.DeleteObject(_bucketName, path);
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // The file does not exist, no need to do anything
        }
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public bool FileExists(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        try
        {
            Google.Apis.Storage.v1.Data.Object storageObject = _storageClient.GetObject(_bucketName, path);
            return storageObject != null;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="fullPathOrUrl" /> is <c>null</c>.</exception>
    public string GetRelativePath(string fullPathOrUrl)
    {
        ArgumentNullException.ThrowIfNull(fullPathOrUrl);

        // test url
        var path = EnsureUrlSeparatorChar(fullPathOrUrl); // ensure url separator char

        // if it starts with the request/URL root path, strip it and trim the starting slash to make it relative
        // eg "/Media/1234/img.jpg" => "1234/img.jpg"
        if (_ioHelper.PathStartsWith(path, _requestRootPath, '/'))
        {
            path = path[_requestRootPath.Length..].TrimStart('/');
        }

        // unchanged - what else?
        return path;
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public string GetFullPath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        path = EnsureUrlSeparatorChar(path);
        return (_ioHelper.PathStartsWith(path, _requestRootPath, '/') ? path : $"{_requestRootPath}/{path}").Trim('/');
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:URI-like return values should not be strings", Justification = "This method is inherited from an interface.")]
    public string GetUrl(string? path)
    {
        ArgumentNullException.ThrowIfNull(path);

        return $"{_requestRootPath}/{EnsureUrlSeparatorChar(path).Trim('/')}";
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public DateTimeOffset GetLastModified(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        Google.Apis.Storage.v1.Data.Object storageObject = _storageClient.GetObject(_bucketName, path);
        DateTimeOffset? dateTimeOffset = storageObject.UpdatedDateTimeOffset;

        if (dateTimeOffset.HasValue)
        {
            return dateTimeOffset.Value;
        }

        throw new InvalidOperationException("The last modified date could not be retrieved.");
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public DateTimeOffset GetCreated(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        Google.Apis.Storage.v1.Data.Object storageObject = _storageClient.GetObject(_bucketName, path);
        DateTimeOffset? dateTimeOffset = storageObject.TimeCreatedDateTimeOffset;

        if (dateTimeOffset.HasValue)
        {
            return dateTimeOffset.Value;
        }

        throw new InvalidOperationException("The create date could not be retrieved.");
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public long GetSize(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        Google.Apis.Storage.v1.Data.Object storageObject = _storageClient.GetObject(_bucketName, path);
        if (storageObject.Size.HasValue)
        {
            if (storageObject.Size.Value > long.MaxValue)
            {
                throw new OverflowException("The size of the object exceeds the maximum value for a long.");
            }
            return (long)storageObject.Size.Value;
        }

        throw new InvalidOperationException("The size of the object could not be retrieved.");
    }

    /// <inheritdoc />
    /// <exception cref="System.ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
    public StorageClient GetStorageClient(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        GoogleCredential credential;
        const string name = GoogleCloudStorageFileSystemOptions.MediaFileSystemName;
        GoogleCloudStorageFileSystemOptions options = _optionsMonitor.Get(name);

        using (var jsonStream = new FileStream(options.CredentialPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(jsonStream);
        }

        return StorageClient.Create(credential);
    }

    /// <inheritdoc />
    //public IFileProvider Create() => throw new NotImplementedException();
    public IFileProvider Create() => new GoogleCloudStorageFileProvider(_credential, _bucketName, _optionsMonitor, "/"); //TODO USE OPTIONS for root path?

    private static string GetRequestRootPath(GoogleCloudStorageFileSystemOptions options, IHostingEnvironment hostingEnvironment)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(hostingEnvironment);

        return hostingEnvironment.ToAbsolute(options.VirtualPath);
    }

    private static string EnsureUrlSeparatorChar(string path)
        => path.Replace("\\", "/", StringComparison.InvariantCultureIgnoreCase);

    private string GetContentType(string path)
    {
        //TODO IS THIS THE RIGHT PLACE
        // Implement logic to get content type based on file extension or use a library
        // For example, you could use Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream"; // default content type if none is found
        }
        return contentType;
    }
}
