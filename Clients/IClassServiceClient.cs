using AdminService.Models;

namespace AdminService.Clients;

public interface IClassServiceClient
{
    Task AddAdminToCenter(
        string centerId,
        string firstName,
        string lastName,
        string adminId,
        AdminRole role);
}