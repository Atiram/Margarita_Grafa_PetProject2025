namespace DocumentService.BBL.Services.Interfaces;
public interface IAzureBlobService
{
    Task<string> UploadFileAsync(string localFilePath, string blobName, CancellationToken cancellationToken = default);
    Task<string> UploadFileFromMemoryAsync(byte[] fileBytes, string blobName, CancellationToken cancellationToken = default);
    Task<bool> DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default);
    Task DownloadFileAsync(string blobName, string downloadFilePath, CancellationToken cancellationToken = default);
}
