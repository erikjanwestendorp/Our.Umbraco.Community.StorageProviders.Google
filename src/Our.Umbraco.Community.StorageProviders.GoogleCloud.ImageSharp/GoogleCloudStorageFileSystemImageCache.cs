// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob.ImageSharp/AzureBlobFileSystemImageCache.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Resolvers;
using Umbraco.Extensions;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.ImageSharp;

public sealed class GoogleCloudStorageFileSystemImageCache : IImageCache
{
    public GoogleCloudStorageFileSystemImageCache(IOptionsMonitor<GoogleCloudStorageFileSystemOptions> options, string name, string? bucketRootPath)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(name);

        GoogleCloudStorageFileSystemOptions fileSystemOptions = options.Get(name);
    }

    public Task<IImageCacheResolver> GetAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync(string key, Stream stream, ImageCacheMetadata metadata)
    {
        throw new NotImplementedException();
    }

    private static string? GetBucketRootPath(string? bucketRootPath, GoogleCloudStorageFileSystemOptions? options = null)
    {
        var path = bucketRootPath ?? options?.BucketRootPath;

        return string.IsNullOrEmpty(path) ? null : path.EnsureEndsWith('/');
    }
}
