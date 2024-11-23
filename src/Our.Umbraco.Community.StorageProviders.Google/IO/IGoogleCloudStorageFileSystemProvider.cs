namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;

/// <summary>
/// The Google Cloud file system provider
/// </summary>
public interface IGoogleCloudStorageFileSystemProvider
{
    /// <summary>
    /// Get the file system by its <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name of the <see cref="IGoogleCloudStorageFileSystem" />.</param>
    /// <returns>
    /// The <see cref="IGoogleCloudStorageFileSystem" />.
    /// </returns>
    IGoogleCloudStorageFileSystem GetFileSystem(string name);
}
