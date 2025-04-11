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
    public async Task<FileModel> GetFile(string id)
    {
        var file = await fileService.GetByIdAsync(id);
        return file;
    }

    [HttpGet]
    public async Task<IEnumerable<FileModel>> GetAllFiles()
    {
        var files = await fileService.GetAllAsync();
        return files;
    }

    [HttpPost("download")]
    public async Task DownloadFile(FileModel fileModel)
    {
        await fileService.DownloadFileAsync(fileModel);
    }

    [HttpPost]
    public async Task<FileModel> CreateFile(CreateFileRequest createFileRequest)
    {
        var createdFile = await fileService.CreateAsync(createFileRequest);
        return createdFile;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(string id)
    {
        var deleted = await fileService.DeleteAsync(id);
        return deleted ? Ok() : NotFound(string.Format(NotificationMessages.NotFoundErrorMessage, id));
    }
}

