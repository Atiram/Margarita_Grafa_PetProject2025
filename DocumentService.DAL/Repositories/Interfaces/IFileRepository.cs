using DocumentService.DAL.Entities;

namespace DocumentService.DAL.Repositories.Interfaces;
public interface IFileRepository
{
    Task<FileEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<FileEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FileEntity> CreateAsync(FileEntity fileEntity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
