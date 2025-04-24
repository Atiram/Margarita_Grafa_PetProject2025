using Clinic.Domain;
using DocumentService.BBL.Models;

namespace DocumentService.BBL.Services.Interfaces;
public interface IFileService
{
    Task<FileModel> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<FileModel> GetByReferenceItemIdAsync(string id, CancellationToken cancellationToken);
    Task<List<FileModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<FileModel> CreateAsync(CreateFileRequest documentModel, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
    Task<bool> DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken);
    Task<bool> DeleteByReferenceItemIdAsync(string id, CancellationToken cancellationToken);
}
