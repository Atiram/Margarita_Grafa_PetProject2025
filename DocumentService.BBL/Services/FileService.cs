using System.Diagnostics;
using System.Text;
using System.Threading;
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
    public async Task<FileModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id, cancellationToken);
        return mapper.Map<FileModel>(documentEntity);
    }

    public async Task<List<FileModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documentEntities = await documentRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<FileModel>>(documentEntities);
    }

    public async Task<FileModel> CreateAsync(CreateFileRequest createFileRequest, string? localFilePath, CancellationToken cancellationToken = default)
    {
        var documentEntity = mapper.Map<FileEntity>(createFileRequest);
        if (string.IsNullOrEmpty(createFileRequest.BlobName))
        {
            throw new ArgumentException(NotificationMessages.NoBlobNameErrorMessage);
        }
        if (!string.IsNullOrEmpty(localFilePath))
        {
            documentEntity.StorageLocation = await blobStorageService.UploadFileAsync(localFilePath, createFileRequest.BlobName, cancellationToken);
        }
        else
        {
            var fileBytes = GenerateInMemoryTextFile();
            documentEntity.StorageLocation = await blobStorageService.UploadFileFromMemoryAsync(fileBytes, createFileRequest.BlobName, cancellationToken);
        }
        var createdDocumentEntity = await documentRepository.CreateAsync(documentEntity, cancellationToken);
        return mapper.Map<FileModel>(createdDocumentEntity);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id, cancellationToken);
        if (documentEntity == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        bool isFileDeleted = await blobStorageService.DeleteBlobAsync(documentEntity.BlobName, cancellationToken);
        if (!isFileDeleted)
        {
            throw new Exception(NotificationMessages.NotDeletedErrorMessage);
        }
        return await documentRepository.DeleteAsync(id, cancellationToken);
    }

    //public async Task DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken = default)
    //{
    //    var documentModel = await GetByIdAsync(id, cancellationToken) ??
    //        throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));

    //    if (string.IsNullOrEmpty(downloadFilePath))
    //    {
    //        Process.Start(new ProcessStartInfo
    //        {
    //            FileName = documentModel.StorageLocation,
    //            UseShellExecute = true
    //        });
    //    }
    //    else if (!string.IsNullOrEmpty(documentModel.BlobName))
    //    {
    //        await blobStorageService.DownloadFileAsync(documentModel.BlobName, downloadFilePath, cancellationToken);
    //    }
    //}
    public async Task<bool> DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken = default)
    {
        var documentModel = await GetByIdAsync(id, cancellationToken) ??
                            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        if (string.IsNullOrWhiteSpace(downloadFilePath))
        {
            throw new ArgumentException(NotificationMessages.NoDownloadFilePathErrorMessage);
        }
        if (string.IsNullOrWhiteSpace(documentModel.BlobName))
        {
            throw new InvalidOperationException(NotificationMessages.NoBlobNameErrorMessage);
        }
        await blobStorageService.DownloadFileAsync(documentModel.BlobName, downloadFilePath, cancellationToken);
        return true;

    }

    public async Task<bool> OpenFileInBrowserAsync(string id, CancellationToken cancellationToken = default)
    {
        var documentModel = await GetByIdAsync(id, cancellationToken) ??
                            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        Process.Start(new ProcessStartInfo
        {
            FileName = documentModel.StorageLocation,
            UseShellExecute = true
        });
        return true;
    }
 
    //this method initates file content in memory, will be changed later
    private byte[] GenerateInMemoryTextFile()
    {
        string fileContent = "Test file for container";
        return Encoding.UTF8.GetBytes(fileContent);
    }
}
