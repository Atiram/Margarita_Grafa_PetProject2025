using Clinic.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentService.DAL.Entities;
public class FileEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required DocumentType DocumentType { get; set; }
    public required string BlobName { get; set; }
    public string? StorageLocation { get; set; }
    public string? ReferenceItemId { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UploadedDate { get; set; }
}
