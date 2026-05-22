using AdminService.DTOs;
using AdminService.Models;

namespace AdminService.Clients;

public class ClassServiceClient
{
    private readonly HttpClient _httpClient;

    public ClassServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddAdminToCenter(string centerId, string FirstName, string LastName, string adminId, AdminRole role)
    {
        var dto = new AdminCenterDto
        {
            AdminId = adminId,
            FirstName = FirstName,
            LastName = LastName,
            Role = role.ToString()
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"api/centers/{centerId}/admins", dto);

        response.EnsureSuccessStatusCode();
    }
}