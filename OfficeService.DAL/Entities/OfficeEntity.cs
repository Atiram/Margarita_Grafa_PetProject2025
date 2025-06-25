using Clinic.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficeService.DAL.Entities;
public class OfficeEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
    public required string OfficeNumber { get; set; }
    public required string RegistryPhoneNumber { get; set; }
    public OfficeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
