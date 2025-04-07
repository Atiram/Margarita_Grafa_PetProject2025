using Azure.Storage.Blobs.Models;
using DocumentService.DAL.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentService.DAL.Entities;
public class DocumentEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public DocumentType DocumentType { get; set; }
    public string FileName { get; set; }
    public string StorageLocation { get; set; }
    public DateTime UploadedDate { get; set; }
}
