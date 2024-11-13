using Google.Cloud.Storage.V1;
using Umbraco.Cms.Core.IO;

namespace Our.Umbraco.Community.StorageProviders.Google.IO;
/// <summary>
/// Represents a Google Cloud Storage file system.
/// </summary>
public interface IGoogleCloudStorageFileSystem : IFileSystem
{
    /// <summary>
    /// Get the <see cref="StorageClient" />.
    /// </summary>
    /// <param name="path">The relative path to the storage.</param>
    /// <returns>
    /// A <see cref="StorageClient" />.
    /// </returns>
    StorageClient GetStorageClient(string path);
}