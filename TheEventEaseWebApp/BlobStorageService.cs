using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TheEventEaseWebApp
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("ContainerConnection"));
            _containerName = configuration.GetConnectionString("ContainerName");
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            // Get the container client
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Ensure container exists
            await containerClient.CreateIfNotExistsAsync();

            // Get the blob client
            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file
            await blobClient.UploadAsync(fileStream, overwrite: true);

            /*
             * A Shared Access Signature (SAS) token
             * is a security token that grants limited access to specific resources or operations
             * without requiring the sharing of the storage account key
             */
            // Generate the SAS token specific to our blob storage
            var sasToken = GenerateSasToken(blobClient);

            // Construct the image URL with SAS token
            var imageUrlWithSas = $"{blobClient.Uri}?{sasToken}"; // append the url with the SAS token to retrieve from azure storage

            // Return the image URL with SAS token
            return imageUrlWithSas;
        }

        private string GenerateSasToken(BlobClient blobClient)
        {
            // Create a SAS builder to specify permissions and expiry
            // This creates the token
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = blobClient.Name,
                Resource = "b", // 'b' for blob
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(1) // Token expiration time (1 year for consistency with your controllers)
            };

            // Set the permissions for the SAS token (read permission)
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Generate the SAS token using the blob's URI
            var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;
            return sasToken;
        }
    }
}