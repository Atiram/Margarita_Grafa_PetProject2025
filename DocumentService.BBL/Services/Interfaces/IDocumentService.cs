using DocumentService.BBL.Models;

namespace DocumentService.BBL.Services.Interfaces;
public interface IDocumentService
{
    Task<DocumentModel> GetByIdAsync(string id);
    Task<List<DocumentModel>> GetAllAsync();
    Task<DocumentModel> CreateAsync(DocumentModel documentModel);
    Task<bool> DeleteAsync(string id);
}
