using DocumentService.DAL.Entities;

namespace DocumentService.DAL.Repositories.Interfaces;
public interface IDocumentRepository
{
    Task<DocumentEntity> GetByIdAsync(string id);
    Task<List<DocumentEntity>> GetAllAsync();
    Task<DocumentEntity> CreateAsync(DocumentEntity documentEntity);
    Task<bool> DeleteAsync(string id);
}
