using Clinic.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Clinic.Domain;
public class CreateFileRequest
{
    public DocumentType DocumentType { get; set; }
    public required string BlobName { get; set; }
    public required string ReferenceItemId { get; set; }
    public IFormFile? File { get; set; }
    public byte[]? InMemoryFile { get; set; }
}
