using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.Models;

public enum AdminRole {Receptionist, Instructor, Centerleder, Pt}
public class Admin
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    
    [BsonRepresentation(BsonType.String)]
    public AdminRole Role { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string CenterId { get; set; } = "";
}
