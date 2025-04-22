using Clinic.Domain.Enums;

namespace DocumentService.BBL.Models.Requests;
public class CreateFileRequest
{
    public DocumentType DocumentType { get; set; }
    public required string BlobName { get; set; }
}
