using AutoMapper;
using DocumentService.BBL.Models;
using DocumentService.BBL.Services.Interfaces;
using DocumentService.DAL.Entities;
using DocumentService.DAL.Repositories.Interfaces;

namespace DocumentService.BBL.Services;
public class FileService(IFileRepository documentRepository, IMapper mapper) : IFileService
{
    public async Task<FileModel> GetByIdAsync(string id)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id);
        return mapper.Map<FileModel>(documentEntity);
    }

    public async Task<List<FileModel>> GetAllAsync()
    {
        var documentEntities = await documentRepository.GetAllAsync();
        return mapper.Map<List<FileModel>>(documentEntities);
    }

    public async Task<FileModel> CreateAsync(FileModel documentModel)
    {
        var documentEntity = mapper.Map<FileEntity>(documentModel);
        var createdDocumentEntity = await documentRepository.CreateAsync(documentEntity);
        return mapper.Map<FileModel>(createdDocumentEntity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await documentRepository.DeleteAsync(id);
    }
}
