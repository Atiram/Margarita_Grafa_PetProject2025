using DocumentService.DAL.Enums;

namespace DocumentService.BBL.Models;
public class FileModel
{
    public string Id { get; set; } = null!;
    public DocumentType DocumentType { get; set; }
    public string FileName { get; set; }
    public string StorageLocation { get; set; }
    public DateTime UploadedDate { get; set; }
}
