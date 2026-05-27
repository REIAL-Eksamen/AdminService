using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.DTOs;

public class AdminCenterDto
{
    public string AdminId { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Role { get; set; } = "";
}