using AdminService.Models;

namespace AdminService.Repositories;

/// <summary>
/// Definerer dataaccesskontrakten for Admin-entiteter.
/// Adskiller databaselogik fra forretningslogik og gør repository'et udskifteligt i tests.
/// </summary>
public interface IAdminRepository
{
    Task<List<Admin>> GetAll();
    Task<Admin?> GetById(string id);
    /// <summary>Henter alle admins tilknyttet et bestemt center.</summary>
    Task<List<Admin>> GetByCenterAsync(string centerId);
    Task Create(Admin admin);
    /// <summary>Erstatter hele admin-dokumentet i databasen med det opdaterede objekt.</summary>
    Task Update(string id, Admin admin);
    Task Delete(string id);
}