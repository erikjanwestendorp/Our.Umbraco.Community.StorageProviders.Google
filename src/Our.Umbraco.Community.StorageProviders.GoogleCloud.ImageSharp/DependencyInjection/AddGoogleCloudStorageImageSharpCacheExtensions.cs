// This code is adapted from the Umbraco.StorageProviders project.
// Original source: https://github.com/umbraco/Umbraco.StorageProviders/blob/develop/src/Umbraco.StorageProviders.AzureBlob.ImageSharp/DependencyInjection/AddAzureBlobImageSharpCacheExtensions.cs
// Adapted and modified in accordance with the terms of the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;
using SixLabors.ImageSharp.Web.Caching;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.ImageSharp.DependencyInjection;

public static class AddGoogleCloudStorageImageSharpCacheExtensions
{
    private const string BucketRootPath = "cache";

    public static IUmbracoBuilder AddGoogleCloudStorageImageSharpCache(this IUmbracoBuilder builder)
        => builder.AddInternal(GoogleCloudStorageFileSystemOptions.MediaFileSystemName, BucketRootPath);

    internal static IUmbracoBuilder AddInternal(this IUmbracoBuilder builder, string name, string? bucketRootPath)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddUnique<IImageCache>(provider => new GoogleCloudStorageFileSystemImageCache(provider.GetRequiredService<IOptionsMonitor<GoogleCloudStorageFileSystemOptions>>(), name, bucketRootPath));

        return builder;
    }

}
