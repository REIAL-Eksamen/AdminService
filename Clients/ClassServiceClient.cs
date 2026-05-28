using AdminService.DTOs;
using AdminService.Models;

namespace AdminService.Clients;

/// <summary>
/// HTTP-klient der kommunikerer med ClassService's API.
/// Bruges til at synkronisere admin-data på tværs af de to services,
/// f.eks. når en admin tildeles et center i AdminService.
/// </summary>
public class ClassServiceClient : IClassServiceClient
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// HttpClient injiceres og konfigureres med BaseAddress i Program.cs
    /// via <c>builder.Services.AddHttpClient&lt;IClassServiceClient, ClassServiceClient&gt;</c>.
    /// </summary>
    public ClassServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Tilknytter en admin til et center i ClassService, så ClassService
    /// kender til instructorens navn og rolle uden at skulle kalde AdminService tilbage.
    /// </summary>
    /// <param name="centerId">ID på det center adminen tilknyttes.</param>
    /// <param name="FirstName">Adminens fornavn — gemmes direkte i ClassService for at undgå opslag.</param>
    /// <param name="LastName">Adminens efternavn.</param>
    /// <param name="adminId">AdminService's MongoDB-ID for adminen.</param>
    /// <param name="role">Adminens rolle, serialiseres til string inden afsendelse.</param>
    /// <exception cref="HttpRequestException">Kastes hvis ClassService returnerer en fejlkode.</exception>
    public async Task AddAdminToCenter(string centerId, string FirstName, string LastName, string adminId, AdminRole role)
    {
        // Byg DTO med de data ClassService skal bruge for at registrere adminen på centeret
        var dto = new AdminCenterDto
        {
            AdminId = adminId,
            FirstName = FirstName,
            LastName = LastName,
            // AdminRole er en enum — konverteres til string så JSON-payload er læsbart
            Role = role.ToString()
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"api/centers/{centerId}/admins", dto);

        // Kast exception ved HTTP-fejl, så kalderen ved at synkroniseringen fejlede
        response.EnsureSuccessStatusCode();
    }
}