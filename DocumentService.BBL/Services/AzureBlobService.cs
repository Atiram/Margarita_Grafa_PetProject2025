using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocumentService.BBL.Services;
public class AzureBlobService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlobService(string connectionString, string containerName)
    {
        _connectionString = connectionString;
        _containerName = containerName;
    }

    public async Task<BlobClient> GetBlobClientAsync(string blobName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
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
        await File.WriteAllBytesAsync(downloadFilePath, downloadResult.Content.ToArray());
    }
}


