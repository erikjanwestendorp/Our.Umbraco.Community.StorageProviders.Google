using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.Google.IO;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.DependencyInjection;

namespace Our.Umbraco.Community.StorageProviders.Google.DependencyInjection;

/// <summary>
/// Extensions methods to help registering Google Cloud Storage file systems.
/// </summary>
public static class GoogleCloudFileSystemExtensions
{
    /// <summary>
    /// Registers a <see cref="IGoogleCloudStorageFileSystemProvider" /> in the <see cref="IServiceCollection" />, with it's configuration
    /// loaded from <c>Umbraco:Storage:GoogleCloud:{name}</c> where {name} is the value of the <paramref name="name" /> parameter.
    /// </summary>
    /// <param name="builder">The <see cref="IUmbracoBuilder" />.</param>
    /// <param name="name">The name of the file system.</param>
    /// <param name="configure">An action used to configure the <see cref="GoogleCloudStorageFileSystemOptions" />.</param>
    /// <returns>
    /// The <see cref="IUmbracoBuilder" />.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="builder" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <c>null</c>.</exception>
    internal static IUmbracoBuilder AddInternal(this IUmbracoBuilder builder, string name, Action<OptionsBuilder<GoogleCloudStorageFileSystemOptions>>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

        builder.Services.TryAddSingleton<IGoogleCloudStorageFileSystemProvider, GoogleCloudStorageFileSystemProvider>();

        OptionsBuilder<GoogleCloudStorageFileSystemOptions> optionsBuilder = builder.Services.AddOptions<GoogleCloudStorageFileSystemOptions>(name)
            .BindConfiguration($"Umbraco:Storage:GoogleCloud:{name}")
            .ValidateDataAnnotations();

        configure?.Invoke(optionsBuilder);

        return builder;
    }
}
