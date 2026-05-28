using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminService.Models;

/// <summary>
/// Definerer de mulige roller en admin kan have i systemet.
/// Bruges til at styre hvilke funktioner adminen har adgang til.
/// </summary>
public enum AdminRole { Receptionist, Instruktør, Centerleder, Pt }

/// <summary>
/// Repræsenterer en admin-bruger (medarbejder) i FitLife-systemet.
/// En admin er tilknyttet ét center og har en bestemt rolle.
/// </summary>
public class Admin
{
    /// <summary>
    /// MongoDB ObjectId — genereres automatisk ved oprettelse.
    /// Bruges som instruktør-ID i ClassService.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";

    /// <summary>
    /// Gemmes som string i MongoDB (frem for int) for læsbarhed i databasen.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public AdminRole Role { get; set; }

    /// <summary>
    /// Reference til det center adminen er tilknyttet.
    /// Svarer til et Center-dokument i ClassService's database.
    /// </summary>
    public string CenterId { get; set; } = "";
}