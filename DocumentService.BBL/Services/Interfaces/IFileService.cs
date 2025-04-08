using DocumentService.BBL.Models;

namespace DocumentService.BBL.Services.Interfaces;
public interface IFileService
{
    Task<FileModel> GetByIdAsync(string id);
    Task<List<FileModel>> GetAllAsync();
    Task<FileModel> CreateAsync(FileModel documentModel);
    Task<bool> DeleteAsync(string id);
}
