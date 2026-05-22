using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.DTOs;

public class AdminCenterDto
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string AdminId { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    
    [BsonRepresentation(BsonType.String)]
    public string Role { get; set; } = "";
}