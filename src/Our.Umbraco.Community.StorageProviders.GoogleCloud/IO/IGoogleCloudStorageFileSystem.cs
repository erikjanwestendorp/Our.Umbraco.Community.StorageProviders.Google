// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob/IO/IAzureBlobFileSystem.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Google.Cloud.Storage.V1;
using Umbraco.Cms.Core.IO;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;

/// <summary>
/// Represents a Google Cloud Storage file system.
/// </summary>
public interface IGoogleCloudStorageFileSystem : IFileSystem
{
    /// <summary>
    /// Get the <see cref="StorageClient" />.
    /// </summary>
    /// <returns>
    /// A <see cref="StorageClient" />.
    /// </returns>
    StorageClient GetStorageClient();
}