using System.Diagnostics;
using System.Text;
using AutoMapper;
using Clinic.Domain;
using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;
using DocumentService.BBL.Services.Interfaces;
using DocumentService.DAL.Entities;
using DocumentService.DAL.Repositories.Interfaces;
using MongoDB.Driver;

namespace DocumentService.BBL.Services;
public class FileService(
    IAzureBlobService blobStorageService,
    IFileRepository documentRepository,
    IMapper mapper) : IFileService
{
    public async Task<FileModel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id, cancellationToken);
        return mapper.Map<FileModel>(documentEntity);
    }

    public async Task<List<FileModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var documentEntities = await documentRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<FileModel>>(documentEntities);
    }

    public async Task<FileModel> CreateAsync(CreateFileRequest createFileRequest, string? localFilePath, CancellationToken cancellationToken)
    {
        var documentEntity = mapper.Map<FileEntity>(createFileRequest);
        bool uploadSuccessful = false;

        try
        {
            if (!string.IsNullOrEmpty(localFilePath))
            {
                documentEntity.StorageLocation = await blobStorageService.UploadFileAsync(localFilePath, createFileRequest.BlobName, cancellationToken);
            }
            else
            {
                var fileBytes = GenerateInMemoryTextFile();
                documentEntity.StorageLocation = await blobStorageService.UploadFileFromMemoryAsync(fileBytes, createFileRequest.BlobName, cancellationToken);
            }

            uploadSuccessful = true;
            var createdDocumentEntity = await documentRepository.CreateAsync(documentEntity, cancellationToken);
            return mapper.Map<FileModel>(createdDocumentEntity);
        }
        catch (MongoException mongoEx)
        {
            if (uploadSuccessful)
            {
                await RollbackFileUpload(createFileRequest.BlobName, cancellationToken);
            }
            throw new MongoException(NotificationMessages.WritingBlobErrorMessage, mongoEx);
        }
        catch (Exception ex)
        {
            if (uploadSuccessful)
            {
                await RollbackFileUpload(createFileRequest.BlobName, cancellationToken);
            }
            throw new Exception(NotificationMessages.UploadingFileErrorMessage, ex);
        }
    }

    private async Task RollbackFileUpload(string blobName, CancellationToken cancellationToken)
    {
        try
        {
            await blobStorageService.DeleteBlobAsync(blobName, cancellationToken);
        }
        catch (Exception deleteEx)
        {
            throw new Exception(NotificationMessages.DeletingBlobErrorMessage, deleteEx);
        }
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var documentEntity = await documentRepository.GetByIdAsync(id, cancellationToken);
        if (documentEntity == null)
        {
            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));
        }
        var isFileDeleted = await blobStorageService.DeleteBlobAsync(documentEntity.BlobName, cancellationToken);
        if (!isFileDeleted)
        {
            throw new Exception(NotificationMessages.NotDeletedErrorMessage);
        }
        return await documentRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<bool> DownloadFileAsync(string id, string? downloadFilePath, CancellationToken cancellationToken)
    {
        return string.IsNullOrEmpty(downloadFilePath)
            ? await OpenFileInBrowserAsync(id, cancellationToken)
            : await DownloadAsync(id, downloadFilePath, cancellationToken);
    }

    private async Task<bool> OpenFileInBrowserAsync(string id, CancellationToken cancellationToken)
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

    private async Task<bool> DownloadAsync(string id, string downloadFilePath, CancellationToken cancellationToken)
    {
        var documentModel = await GetByIdAsync(id, cancellationToken) ??
                            throw new Exception(string.Format(NotificationMessages.NotFoundErrorMessage, id));

        if (string.IsNullOrWhiteSpace(documentModel.BlobName))
        {
            throw new InvalidOperationException(NotificationMessages.NoBlobNameErrorMessage);
        }
        await blobStorageService.DownloadFileAsync(documentModel.BlobName, downloadFilePath, cancellationToken);
        return true;
    }

    //this method initates file content in memory, will be changed later
    private byte[] GenerateInMemoryTextFile()
    {
        string fileContent = "Test file for container";
        return Encoding.UTF8.GetBytes(fileContent);
    }
}
