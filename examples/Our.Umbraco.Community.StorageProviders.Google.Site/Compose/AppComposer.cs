using Our.Umbraco.Community.StorageProviders.Google.Site.NotificationHandlers;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.DependencyInjection;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.ImageSharp.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace Our.Umbraco.Community.StorageProviders.Google.Site.Compose;

public class AppComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
        => builder
            .AddGoogleCloudMediaFileSystem()
            .AddGoogleCloudStorageImageSharpCache()
            .AddNotificationHandler<UnattendedInstallNotification, GoogleCloudStorageMediaFileSystemCreateIfNotExistsHandler>();
}
