using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Our.Umbraco.Community.StorageProviders.Google.Site.NotificationHandlers;

public class GoogleCloudStorageMediaFileSystemCreateIfNotExistsHandler(
    IOptionsMonitor<GoogleCloudStorageFileSystemOptions> options,
    ILogger<GoogleCloudStorageFileSystem> logger) : INotificationHandler<UnattendedInstallNotification>
{
    private readonly GoogleCloudStorageFileSystemOptions _options = options.Get(GoogleCloudStorageFileSystemOptions.MediaFileSystemName);

    public void Handle(UnattendedInstallNotification notification) 
        => GoogleCloudStorageFileSystem.CreateIfNotExists(_options, logger);
}
