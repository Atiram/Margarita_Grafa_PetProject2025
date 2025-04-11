using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;

namespace DocumentService.BBL.Services.Interfaces;
public interface IFileService
{
    Task<FileModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<FileModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FileModel> CreateAsync(CreateFileRequest documentModel, string localFilePath, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    //Task DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken = default);
    Task<bool> DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken = default);
    Task<bool> OpenFileInBrowserAsync(string id, CancellationToken cancellationToken = default);

}
