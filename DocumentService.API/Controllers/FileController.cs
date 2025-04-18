using Clinic.Domain;
using DocumentService.BBL.Models;
using DocumentService.BBL.Models.Requests;
using DocumentService.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FileController(IFileService fileService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<FileModel> GetFile(string id, CancellationToken cancellationToken = default)
    {
        var file = await fileService.GetByIdAsync(id, cancellationToken);
        return file;
    }

    [HttpGet]
    public async Task<IEnumerable<FileModel>> GetAllFiles(CancellationToken cancellationToken = default)
    {
        var files = await fileService.GetAllAsync(cancellationToken);
        return files;
    }

    [HttpPost("download")]
    public async Task DownloadFile(string id, string? downloadFilePath, CancellationToken cancellationToken = default)
    {
        await fileService.DownloadFileAsync(id, downloadFilePath ?? string.Empty, cancellationToken);
    }

    [HttpPost]
    public async Task<FileModel> CreateFile(CreateFileRequest createFileRequest, string? localFilePath, CancellationToken cancellationToken = default)
    {
        var createdFile = await fileService.CreateAsync(createFileRequest, localFilePath ?? string.Empty, cancellationToken);
        return createdFile;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(string id, CancellationToken cancellationToken = default)
    {
        var deleted = await fileService.DeleteAsync(id, cancellationToken);
        return deleted ? Ok() : NotFound(string.Format(NotificationMessages.NotFoundErrorMessage, id));
    }
}

