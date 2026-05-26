using AdminService.Clients;
using AdminService.Models;
using AdminService.Repositories;

namespace AdminService.Services;

public class AdminService
{
    private readonly IAdminRepository _admins;
    private readonly IClassServiceClient _classServiceClient;

    public AdminService(IAdminRepository admins, IClassServiceClient classServiceClient)
    {
        _admins = admins;
        _classServiceClient = classServiceClient;
    }

    public Task<List<Admin>> GetAllAsync() => _admins.GetAll();

    public Task<Admin?> GetByIdAsync(string id) => _admins.GetById(id);

    public async Task<Admin> CreateAsync(Admin admin)
    {
        await _admins.Create(admin);
        await _classServiceClient.AddAdminToCenter(
            admin.CenterId, admin.FirstName, admin.LastName, admin.Id, admin.Role);
        return admin;
    }

    public async Task<bool> UpdateAsync(string id, Admin updated)
    {
        var existing = await _admins.GetById(id);
        if (existing is null) return false;

        updated.Id = id;
        await _admins.Update(id, updated);
        await _classServiceClient.AddAdminToCenter(
            updated.CenterId, updated.FirstName, updated.LastName, updated.Id, updated.Role);
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _admins.GetById(id);
        if (existing is null) return false;

        await _admins.Delete(id);
        return true;
    }
}