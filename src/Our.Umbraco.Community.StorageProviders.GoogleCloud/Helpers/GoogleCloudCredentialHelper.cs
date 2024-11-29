using Google.Apis.Auth.OAuth2;

namespace Our.Umbraco.Community.StorageProviders.GoogleCloud.Helpers;

internal static class GoogleCloudCredentialHelper
{
    public static GoogleCredential LoadCredential(string credentialPath)
    {
        if (string.IsNullOrWhiteSpace(credentialPath))
        {
            throw new ArgumentException("Credential path cannot be null or empty.", nameof(credentialPath));
        }

        if (!File.Exists(credentialPath))
        {
            throw new FileNotFoundException("Credential file not found.", credentialPath);
        }

        using var jsonStream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read);
        return GoogleCredential.FromStream(jsonStream);
    }
}
