// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/AzureBlobFileProvider.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.Extensions;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud;
/// <summary>
/// Represents a read-only Google Cloud Storage file provider.
/// </summary>
/// <seealso cref="IFileProvider" />
public sealed class GoogleCloudStorageFileProvider : IFileProvider
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly string? _bucketRootPath;
    private readonly GoogleCloudStorageFileSystemOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleCloudStorageFileProvider" /> class.
    /// </summary>
    /// <param name="storageClient">The storage client.</param>
    /// <param name="optionsMonitor">The options monitor providing configuration for the Google Cloud Storage file system.</param>
    /// <param name="bucketRootPath">The bucket root path.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="storageClient" /> is <c>null</c>.</exception>
    public GoogleCloudStorageFileProvider(StorageClient storageClient, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, string? bucketRootPath = null)
    {
        ArgumentNullException.ThrowIfNull(storageClient);
        ArgumentNullException.ThrowIfNull(optionsMonitor);
        
        var options = optionsMonitor.Get(GoogleCloudStorageFileSystemOptions.MediaFileSystemName);

        _storageClient = storageClient;
        _options = options;
        _bucketName = options.BucketName;
        _bucketRootPath = bucketRootPath?.Trim(Constants.CharArrays.ForwardSlash);
    }

    /// <inheritdoc />
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = GetFullPath(subpath);

        var options = new ListObjectsOptions { Delimiter = "/" };

        var objects = _storageClient.ListObjects(_options.BucketName, "/", options).ToList();

        return objects.Count == 0
            ? NotFoundDirectoryContents.Singleton
            : new GoogleCloudDirectoryContents(_storageClient, _options.BucketName, objects.Select(obj =>
                obj.Name.EndsWith("/")
                    ? new GoogleCloudStorageItem { IsPrefix = true, Prefix = obj.Name }
                    : new GoogleCloudStorageItem { IsPrefix = false, Object = obj }).ToList());
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        subpath = subpath.RemoveFirstIfEquals('/');
        var path = GetFullPath(subpath);
        var obj = _storageClient.GetObject(_bucketName, path);
        return new GoogleCloudStorageItemInfo(_storageClient, _bucketName, obj);
    }

    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    private string GetFullPath(string subPath) => _bucketRootPath + subPath.EnsureStartsWith('/');
}
