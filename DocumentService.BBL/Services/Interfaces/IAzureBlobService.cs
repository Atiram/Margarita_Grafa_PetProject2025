using Azure.Storage.Blobs;

namespace DocumentService.BBL.Services.Interfaces;
public interface IAzureBlobService
{
    Task<BlobClient> GetBlobClientAsync(string blobName);
    Task<string> UploadFileAsync(string localFilePath, string blobName);
    Task<string> UploadFileFromMemoryAsync(byte[] fileBytes, string blobName);
    Task<bool> DeleteBlobAsync(string blobName);
    Task DownloadFileAsync(string blobName, string downloadFilePath);
}
