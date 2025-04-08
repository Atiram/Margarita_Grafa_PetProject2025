using DocumentService.DAL.Entities;

namespace DocumentService.DAL.Repositories.Interfaces;
public interface IFileRepository
{
    Task<FileEntity> GetByIdAsync(string id);
    Task<List<FileEntity>> GetAllAsync();
    Task<FileEntity> CreateAsync(FileEntity documentEntity);
    Task<bool> DeleteAsync(string id);
}
