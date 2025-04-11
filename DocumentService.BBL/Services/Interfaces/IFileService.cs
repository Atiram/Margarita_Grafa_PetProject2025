using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;

namespace DocumentService.BBL.Services.Interfaces;
public interface IFileService
{
    Task<FileModel> GetByIdAsync(string id);
    Task<List<FileModel>> GetAllAsync();
    Task<FileModel> CreateAsync(CreateFileRequest documentModel);
    Task<bool> DeleteAsync(string id);
    Task DownloadFileAsync(FileModel documentModel);
}
