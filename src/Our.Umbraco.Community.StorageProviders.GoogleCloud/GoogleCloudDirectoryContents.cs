// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/AzureBlobDirectoryContents.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;
using System.Collections;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud;
/// <summary>
/// Represents a virtual hierarchy of Google Cloud Storage blobs.
/// </summary>
/// <seealso cref="IDirectoryContents" />
public class GoogleCloudDirectoryContents : IDirectoryContents
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly IReadOnlyCollection<GoogleCloudStorageItem> _items;

    public GoogleCloudDirectoryContents(StorageClient storageClient, string bucketName, IReadOnlyCollection<GoogleCloudStorageItem> items)
    {
        _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        _items = items ?? throw new ArgumentNullException(nameof(items));

        Exists = items.Count > 0;
    }

    /// <inheritdoc />
    public bool Exists { get; }

    /// <inheritdoc />
    public IEnumerator<IFileInfo> GetEnumerator()
        => _items.Select<GoogleCloudStorageItem, IFileInfo>(x => x.IsPrefix
            ? new GoogleCloudStoragePrefixInfo(x.Prefix!)
            : new GoogleCloudStorageItemInfo(_storageClient, _bucketName, x.Object!)).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Represents a Google Cloud Storage prefix (virtual directory).
/// </summary>
public class GoogleCloudStoragePrefixInfo(string prefix) : IFileInfo
{
    public bool Exists => true;
    public long Length => -1;
    public string PhysicalPath => string.Empty;
    public string Name { get; } = prefix;
    public DateTimeOffset LastModified => DateTimeOffset.UtcNow;
    public bool IsDirectory { get; } = true;
    public Stream CreateReadStream() => throw new NotSupportedException();
}