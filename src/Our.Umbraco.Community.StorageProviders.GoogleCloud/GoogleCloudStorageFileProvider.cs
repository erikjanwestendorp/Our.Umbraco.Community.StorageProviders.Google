// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/AzureBlobFileProvider.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.Extensions;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.Helpers;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud;
/// <summary>
/// Represents a read-only Google Cloud Storage file provider.
/// </summary>
/// /// <seealso cref="Microsoft.Extensions.FileProviders.IFileProvider" />
public sealed class GoogleCloudStorageFileProvider : IFileProvider
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly string? _containerRootPath;
    private readonly GoogleCredential _credential;
    private readonly IOptionsMonitor<GoogleCloudStorageFileSystemOptions> _optionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleCloudStorageFileProvider" /> class.
    /// </summary>
    /// <param name="storageClient">The storage client.</param>
    /// <param name="containerRootPath">The container root path.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="storageClient" /> is <c>null</c>.</exception>
    public GoogleCloudStorageFileProvider(GoogleCredential credential, string bucketName, IOptionsMonitor<GoogleCloudStorageFileSystemOptions> optionsMonitor, string? containerRootPath = null)
    {
        _storageClient = StorageClient.Create(credential);
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        _credential = credential;
        _containerRootPath = containerRootPath?.Trim(Constants.CharArrays.ForwardSlash);
        _optionsMonitor = optionsMonitor;
    }

    /// <inheritdoc />
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = GetFullPath(subpath);

        var options = new ListObjectsOptions { Delimiter = "/" };

        var objects = _storageClient.ListObjects("umbraco", "/", options).ToList();

        return objects.Count == 0
            ? NotFoundDirectoryContents.Singleton
            : new GoogleCloudDirectoryContents(_storageClient, "umbraco", objects.Select(blob =>
                blob.Name.EndsWith("/")
                    ? new GoogleCloudStorageItem { IsPrefix = true, Prefix = blob.Name }
                    : new GoogleCloudStorageItem { IsPrefix = false, Object = blob }).ToList());
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        subpath = subpath.RemoveFirstIfEquals('/');

        var name = GoogleCloudStorageFileSystemOptions.MediaFileSystemName;
        GoogleCloudStorageFileSystemOptions options = _optionsMonitor.Get(name);

        GoogleCredential credential = GoogleCloudCredentialHelper.LoadCredential(options.CredentialPath);

        StorageClient storageClient = StorageClient.Create(credential);

        var obj = storageClient.GetObject(_bucketName, subpath);
        return new GoogleCloudStorageItemInfo(_storageClient, _bucketName, obj);
    }

    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    private string GetFullPath(string subPath) => _containerRootPath + subPath.EnsureStartsWith('/');
}
