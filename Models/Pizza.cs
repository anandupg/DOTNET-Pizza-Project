using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PizzaApi.Models;

public class Pizza
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
