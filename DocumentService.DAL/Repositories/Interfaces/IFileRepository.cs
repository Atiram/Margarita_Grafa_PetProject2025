using DocumentService.DAL.Entities;

namespace DocumentService.DAL.Repositories.Interfaces;
public interface IFileRepository
{
    Task<FileEntity> GetByIdAsync(string id);
    Task<List<FileEntity>> GetAllAsync();
    Task<FileEntity> CreateAsync(FileEntity fileEntity);
    Task<bool> DeleteAsync(string id);
}
