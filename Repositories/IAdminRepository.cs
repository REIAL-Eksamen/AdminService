using AdminService.Models;

namespace AdminService.Repositories;

public interface IAdminRepository
{
    Task<List<Admin>> GetAll();
    Task<Admin?> GetById(string id);
    Task Create(Admin admin);
    Task Update(string id, Admin admin);
    Task Delete(string id);
}