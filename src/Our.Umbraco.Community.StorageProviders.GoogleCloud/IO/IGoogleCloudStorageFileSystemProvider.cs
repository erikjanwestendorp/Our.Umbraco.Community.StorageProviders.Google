// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/IO/IAzureBlobFileSystemProvider.cs
// Adapted and modified in accordance with the terms of the MIT License.

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
