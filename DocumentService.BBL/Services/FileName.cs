using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocumentService.BBL.Services;
internal class FileName
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileName(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadDocumentAsync(Stream documentStream, string fileName, string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob); // Create if it doesn't exist

        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(documentStream, overwrite: true); // Overwrite if it exists

        return blobClient.Uri.ToString(); // Return the URL of the uploaded blob
    }

    public async Task<Stream> DownloadDocumentAsync(string fileName, string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        MemoryStream memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        memoryStream.Position = 0; // Reset position to the beginning
        return memoryStream;
    }

    public async Task DeleteDocumentAsync(string fileName, string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> ListDocumentsAsync(string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        List<string> documentUrls = new List<string>();
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
            documentUrls.Add(blobClient.Uri.ToString());
        }
        return documentUrls;
    }
}
