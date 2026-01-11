using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PizzaApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!; // Plain text for demo only
    public string Role { get; set; } = "User"; // "Admin" or "User"
}
