using AdminService.Models;

namespace AdminService.Services;

/// <summary>
/// Definerer forretningslogikken for admin-håndtering.
/// Implementationen er ansvarlig for at holde AdminService og ClassService synkroniseret.
/// </summary>
public interface IAdminService
{
    Task<List<Admin>> GetAllAsync();
    /// <returns>Null hvis adminen ikke findes.</returns>
    Task<Admin?> GetByIdAsync(string id);
    Task<List<Admin>> GetByCenterAsync(string centerId);
    /// <summary>Opretter adminen i databasen og notificerer ClassService.</summary>
    Task<Admin> CreateAsync(Admin admin);
    /// <summary>Opdaterer adminen i databasen og synkroniserer ændringerne til ClassService.</summary>
    /// <returns>False hvis adminen ikke findes.</returns>
    Task<bool> UpdateAsync(string id, Admin updated);
    /// <returns>False hvis adminen ikke findes.</returns>
    Task<bool> DeleteAsync(string id);
}