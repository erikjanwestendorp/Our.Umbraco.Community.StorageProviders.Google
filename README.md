#Our.Umbraco.Community.StorageProviders.GoogleCloud

Our.Umbraco.Community.StorageProviders.GoogleCloud is a custom storage provider for Umbraco CMS, integrating with Google Cloud Storage to store and manage your media files seamlessly.


## Features
 - Store and retrieve Umbraco media files in Google Cloud Storage.
 - Easily configurable via app settings.
 - Built with performance and scalability in mind.
 - Compatible with Umbraco CMS 15+.

## Installation

Installing through command line:

```bash
dotnet add package Our.Umbraco.Community.StorageProviders.GoogleCloud
```

Or package reference: 

```
<PackageReference Include="Our.Umbraco.Community.StorageProviders.GoogleCloud" />
```

## Configuration

Add your Google Cloud Storage configuration to your appsettings.json file:

```json
{
  "Umbraco": {
    "Storage": {
      "GoogleCloud": {
        "Media": {
          "BucketName": "your-google-cloud-bucket-name",
          "ProjectId": "your-google-cloud-project-id",
          "Credentials": "path-to-your-service-account-key.json"
        }
      }
    }
  }
}
```

Once the configuration is set up, the storage provider will automatically use Google Cloud Storage for managing media files.

## Contributions
Contributions are welcome! If you encounter bugs, have feature requests, or want to improve the code, feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments
This project is based on the [Umbraco.StorageProviders](https://github.com/umbraco/Umbraco.StorageProviders/) repository. Portions of the code were adapted under the MIT License.

## Support
If you have questions or need assistance, please create an issue on the issue tracker.
