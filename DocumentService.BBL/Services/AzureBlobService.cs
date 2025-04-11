using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Clinic.Domain;
using DocumentService.BBL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DocumentService.BBL.Services;
public class AzureBlobService : IAzureBlobService
{
    private readonly string connectionString;
    private readonly string containerName;
    private const string AzureConnectionStringSectionName = "AzureBlobStorage";
    private const string AzureContainerNameSectionName = "BlobStorageContainerName";

    public AzureBlobService(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString(AzureConnectionStringSectionName) ??
            throw new InvalidOperationException(NotificationMessages.ConnectionStringMissingErrorMessage);
        containerName = configuration.GetSection(AzureContainerNameSectionName)?.Value ??
            throw new InvalidOperationException(NotificationMessages.ContainerNameMissingErrorMessage);
    }

    public async Task<BlobClient> GetBlobClientAsync(string blobName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        if (blobName == null)
        {
            throw new NullReferenceException();
        }
        return containerClient.GetBlobClient(blobName);
    }

    public async Task<string> UploadFileAsync(string localFilePath, string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(blobName);
        await blobClient.UploadAsync(localFilePath, true);
        if (blobClient.Uri == null)
        {
            throw new NullReferenceException();
        }
        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadFileFromMemoryAsync(byte[] fileBytes, string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(blobName);

        using (MemoryStream stream = new MemoryStream(fileBytes))
        {
            await blobClient.UploadAsync(stream);
        }
        if (blobClient.Uri == null)
        {
            throw new NullReferenceException();
        }
        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteBlobAsync(string blobName)
    {
        BlobClient blobClient = await GetBlobClientAsync(blobName);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task DownloadFileAsync(string blobName, string downloadFilePath)
    {
        BlobClient blobClient = await GetBlobClientAsync(blobName);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        await File.WriteAllBytesAsync(Path.Combine(downloadFilePath, blobName), downloadResult.Content.ToArray());
    }
}