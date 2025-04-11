using System.Diagnostics;
using System.Text;
using AutoMapper;
using Clinic.Domain;
using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;
using DocumentService.BBL.Services.Interfaces;
using DocumentService.DAL.Entities;
using DocumentService.DAL.Repositories.Interfaces;

namespace DocumentService.BBL.Services;
public class FileService(
    IAzureBlobService blobStorageService,
    IFileRepository documentRepository,
    IMapper mapper) : IFileService
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

    public async Task<FileModel> CreateAsync(CreateFileRequest createFileRequest, string? localFilePath)
    {
        var documentEntity = mapper.Map<FileEntity>(createFileRequest);
        if (string.IsNullOrEmpty(createFileRequest.BlobName))
        {
            throw new ArgumentException(NotificationMessages.NoBlobNameErrorMessage);
        }
        if (!string.IsNullOrEmpty(localFilePath))
        {
            documentEntity.StorageLocation = await blobStorageService.UploadFileAsync(localFilePath, createFileRequest.BlobName);
        }
        else
        {
            byte[] fileBytes = GenerateInMemoryTextFile();
            documentEntity.StorageLocation = await blobStorageService.UploadFileFromMemoryAsync(fileBytes, createFileRequest.BlobName);
        }
        var createdDocumentEntity = await documentRepository.CreateAsync(documentEntity);
        return mapper.Map<FileModel>(createdDocumentEntity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id);
        if (documentEntity == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        bool isFileDeleted = await blobStorageService.DeleteBlobAsync(documentEntity.BlobName);
        if (!isFileDeleted)
        {
            throw new Exception(NotificationMessages.NotDeletedErrorMessage);
        }
        return await documentRepository.DeleteAsync(id);
    }

    public async Task DownloadFileAsync(string id, string downloadFilePath)
    {
        FileModel documentModel = await GetByIdAsync(id);
        if (string.IsNullOrEmpty(downloadFilePath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = documentModel.StorageLocation,
                UseShellExecute = true
            });
        }
        else if (!string.IsNullOrEmpty(documentModel.BlobName))
        {
            await blobStorageService.DownloadFileAsync(documentModel.BlobName, downloadFilePath);
        }
    }

    //this method initates file content in memory, will be changed later
    private byte[] GenerateInMemoryTextFile()
    {
        string fileContent = "Test file for container";
        return Encoding.UTF8.GetBytes(fileContent);
    }
}
