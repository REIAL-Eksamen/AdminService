using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.DTOs;

/// <summary>
/// DTO der sendes til ClassService når en admin tilknyttes et center.
/// Indeholder de oplysninger ClassService har brug for om instruktøren,
/// uden at skulle kalde AdminService tilbage ved hvert opslag.
/// </summary>
public class AdminCenterDto
{
    public string AdminId { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    /// <summary>Rollen sendes som string, da ClassService ikke kender AdminRole-enumet.</summary>
    public string Role { get; set; } = "";
}