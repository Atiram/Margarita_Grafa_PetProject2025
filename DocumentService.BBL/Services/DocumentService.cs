using AutoMapper;
using DocumentService.BBL.Models;
using DocumentService.BBL.Services.Interfaces;
using DocumentService.DAL.Entities;
using DocumentService.DAL.Repositories.Interfaces;

namespace DocumentService.BBL.Services;
public class DocumentService(IDocumentRepository documentRepository, IMapper mapper) : IDocumentService
{
    public async Task<DocumentModel> GetByIdAsync(string id)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id);
        return mapper.Map<DocumentModel>(documentEntity);
    }

    public async Task<List<DocumentModel>> GetAllAsync()
    {
        var documentEntities = await documentRepository.GetAllAsync();
        return mapper.Map<List<DocumentModel>>(documentEntities);
    }

    public async Task<DocumentModel> CreateAsync(DocumentModel documentModel)
    {
        var documentEntity = mapper.Map<DocumentEntity>(documentModel);
        var createdDocumentEntity = await documentRepository.CreateAsync(documentEntity);
        return mapper.Map<DocumentModel>(createdDocumentEntity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await documentRepository.DeleteAsync(id);
    }
}
