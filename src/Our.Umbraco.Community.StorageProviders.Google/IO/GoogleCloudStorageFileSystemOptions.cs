using System.ComponentModel.DataAnnotations;

namespace Our.Umbraco.Community.StorageProviders.Google.IO;
/// <summary>
/// The Google Cloud Storage File System options.
/// </summary>
public sealed class GoogleCloudStorageFileSystemOptions
{
    /// <summary>
    /// The media filesystem name.
    /// </summary>
    public const string MediaFileSystemName = "Media";

    /// <summary>
    /// Gets or sets the path to the credential file.
    /// </summary>
    [Required]
    public required string CredentialPath { get; set; }

    /// <summary>
    /// Gets or sets the bucket name.
    /// </summary>
    [Required]
    public required string BucketName { get; set; }

    /// <summary>
    /// Gets or sets the root path of the container.
    /// </summary>
    public string? ContainerRootPath { get; set; }

    /// <summary>
    /// Gets or sets the virtual path.
    /// </summary>
    [Required]
    public required string VirtualPath { get; set; }
}