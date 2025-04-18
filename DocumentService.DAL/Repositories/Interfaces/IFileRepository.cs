using DocumentService.DAL.Entities;

namespace DocumentService.DAL.Repositories.Interfaces;
public interface IFileRepository
{
    Task<FileEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<FileEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<FileEntity> CreateAsync(FileEntity fileEntity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}
