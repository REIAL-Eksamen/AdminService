using AdminService.Models;

namespace AdminService.Services;

public interface IAdminService
{
    Task<List<Admin>> GetAllAsync();
    Task<Admin?> GetByIdAsync(string id);
    Task<List<Admin>> GetByCenterAsync(string centerId);
    Task<Admin> CreateAsync(Admin admin);
    Task<bool> UpdateAsync(string id, Admin updated);
    Task<bool> DeleteAsync(string id);
}