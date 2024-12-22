using Google.Cloud.Storage.V1;
using Microsoft.Extensions.FileProviders;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud;

/// <summary>
/// Represents a Google Cloud Storage object (file).
/// </summary>
public class GoogleCloudStorageItemInfo : IFileInfo
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly Google.Apis.Storage.v1.Data.Object _object;

    public GoogleCloudStorageItemInfo(StorageClient storageClient, string bucketName, Google.Apis.Storage.v1.Data.Object obj)
    {
        _storageClient = storageClient;
        _bucketName = bucketName;
        _object = obj;

        Name = _object.Name;
        Length = (long)_object.Size!;
        LastModified = _object.UpdatedDateTimeOffset ?? DateTimeOffset.UtcNow;
    }

    public bool Exists => true;
    public long Length { get; }
    public string PhysicalPath => string.Empty;
    public string Name { get; }
    public DateTimeOffset LastModified { get; }
    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        var stream = new MemoryStream();
        _storageClient.DownloadObject(_bucketName, _object.Name, stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    /// <summary>
    /// Parses the name from the file path.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>
    /// The name.
    /// </returns>
    internal static string ParseName(string path) => path[(path.LastIndexOf('/') + 1)..];

}
