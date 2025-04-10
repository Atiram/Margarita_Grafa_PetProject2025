using DocumentService.DAL.Enums;

namespace DocumentService.BBL.Models;
public class FileModel
{
    public string? Id { get; set; }
    public DocumentType DocumentType { get; set; }
    public string? LocalFilePath { get; set; }
    public string? DownloadFilePath { get; set; }
    public required string BlobName { get; set; }
    public string? StorageLocation { get; set; }
    public DateTime UploadedDate { get; set; }
}
