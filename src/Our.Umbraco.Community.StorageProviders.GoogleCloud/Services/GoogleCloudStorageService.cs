using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Our.Umbraco.Community.StorageProviders.GoogleCloud.IO;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.Services;

public class GoogleCloudStorageService(IOptionsMonitor<GoogleCloudStorageFileSystemOptions> options)
{
    private readonly GoogleCloudStorageFileSystemOptions _fileSystemOptions = options.Get(GoogleCloudStorageFileSystemOptions.MediaFileSystemName);

    public StorageClient GetStorageClient()
    {
        var credentialPath = _fileSystemOptions.CredentialPath;

        GoogleCredential? credential;

        if (!string.IsNullOrWhiteSpace(credentialPath) && File.Exists(credentialPath))
        {
            credential = GoogleCredential.FromFile(credentialPath);
        }
        else
        {
            credential = GoogleCredential.GetApplicationDefault();
        }

        return StorageClient.Create(credential);
    }
}
