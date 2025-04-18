namespace DocumentService.BBL.Services.Interfaces;
public interface IAzureBlobService
{
    Task<string> UploadFileAsync(string localFilePath, string blobName, CancellationToken cancellationToken);
    Task<string> UploadFileFromMemoryAsync(byte[] fileBytes, string blobName, CancellationToken cancellationToken);
    Task<bool> DeleteBlobAsync(string blobName, CancellationToken cancellationToken);
    Task DownloadFileAsync(string blobName, string downloadFilePath, CancellationToken cancellationToken);
}
