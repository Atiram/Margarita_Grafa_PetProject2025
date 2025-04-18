﻿using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;

namespace DocumentService.BBL.Services.Interfaces;
public interface IFileService
{
    Task<FileModel> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<FileModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<FileModel> CreateAsync(CreateFileRequest documentModel, string localFilePath, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
    Task<bool> DownloadFileAsync(string id, string downloadFilePath, CancellationToken cancellationToken);
}
