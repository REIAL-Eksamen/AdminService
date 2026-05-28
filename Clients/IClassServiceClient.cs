using AdminService.Models;

namespace AdminService.Clients;

/// <summary>
/// Definerer kontrakten for kommunikation med ClassService.
/// Abstraktion gør det muligt at mocke klienten i unit tests.
/// </summary>
public interface IClassServiceClient
{
    /// <summary>
    /// Registrerer eller opdaterer en admin på et center i ClassService,
    /// så ClassService kan vise instruktørens navn på hold uden at kalde AdminService.
    /// </summary>
    Task AddAdminToCenter(
        string centerId,
        string firstName,
        string lastName,
        string adminId,
        AdminRole role);
}