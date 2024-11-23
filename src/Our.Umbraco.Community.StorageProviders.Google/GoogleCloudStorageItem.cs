namespace Our.Umbraco.Community.StorageProviders.GoogleCloud;

/// <summary>
/// Represents a Google Cloud Storage item (object or prefix).
/// </summary>
public class GoogleCloudStorageItem
{
    public bool IsPrefix { get; set; }
    public string? Prefix { get; set; }
    public Google.Apis.Storage.v1.Data.Object? Object { get; set; }
}