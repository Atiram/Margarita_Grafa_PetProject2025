using DocumentService.DAL.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentService.DAL.Entities;
public class FileEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required DocumentType DocumentType { get; set; }
    public string? LocalFilePath { get; set; }
    public string? DownloadFilePath { get; set; }
    public required string BlobName { get; set; }
    public string? StorageLocation { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UploadedDate { get; set; }
}
