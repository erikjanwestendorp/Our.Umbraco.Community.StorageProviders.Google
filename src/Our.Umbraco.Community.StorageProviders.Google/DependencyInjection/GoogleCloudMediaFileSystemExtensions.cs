using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.Google.IO;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.DependencyInjection;

namespace Our.Umbraco.Community.StorageProviders.Google.DependencyInjection;
/// <summary>
/// Extension methods to help registering Google Cloud Storage file systems for Umbraco media.
/// </summary>
public static class GoogleCloudMediaFileSystemExtensions
{
    /// <summary>
    /// Registers an <see cref="IGoogleCloudStorageFileSystem"/> and it's dependencies configured for media./>
    /// </summary>
    /// <param name="builder"> The <see cref="IUmbracoBuilder"/>.</param>
    /// <returns>
    /// The <see cref="IUmbracoBuilder" />.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="builder" /> is <c>null</c>.</exception>
    public static IUmbracoBuilder AddGoogleCloudMediaFileSystem(this IUmbracoBuilder builder)
        => builder.AddInternal();

    /// <summary>
    /// Registers a <see cref="IGoogleCloudStorageFileSystemProvider" /> and it's dependencies configured for media.
    /// </summary>
    /// <param name="builder">The <see cref="IUmbracoBuilder" />.</param>
    /// <param name="configure">An action used to configure the <see cref="GoogleCloudStorageFileSystemOptions" />.</param>
    /// <returns>
    /// The <seealso cref="IUmbracoBuilder" />
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    internal static IUmbracoBuilder AddInternal(this IUmbracoBuilder builder, Action<OptionsBuilder<GoogleCloudStorageFileSystemOptions>>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddInternal(GoogleCloudStorageFileSystemOptions.MediaFileSystemName, optionsBuilder =>
        {
            optionsBuilder.Configure<IOptions<GlobalSettings>>((options, globalSettings) => options.VirtualPath = globalSettings.Value.UmbracoMediaPath);
            configure?.Invoke(optionsBuilder);
        });

        builder.SetMediaFileSystem(provider => provider.GetRequiredService<IGoogleCloudStorageFileSystemProvider>().GetFileSystem(GoogleCloudStorageFileSystemOptions.MediaFileSystemName));

        return builder;
    }
}