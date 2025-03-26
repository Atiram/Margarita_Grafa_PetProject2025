using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthenticationService.DAL.Entities;
public class UserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
}
