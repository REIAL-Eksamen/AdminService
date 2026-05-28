using AdminService.Clients;
using AdminService.Models;
using AdminService.Repositories;

namespace AdminService.Services;

/// <summary>
/// Forretningslogik for admin-håndtering.
/// Sørger for at AdminDB og ClassService altid er synkroniserede
/// når en admin oprettes eller opdateres.
/// </summary>
public class AdminService : IAdminService
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

    public Task<List<Admin>> GetByCenterAsync(string centerId) =>
        _admins.GetByCenterAsync(centerId);

    /// <summary>
    /// Opretter adminen i AdminDB og synkroniserer oplysningerne til ClassService,
    /// så ClassService kan vise instruktørens navn på hold uden at kalde AdminService.
    /// </summary>
    public async Task<Admin> CreateAsync(Admin admin)
    {
        await _admins.Create(admin);

        // Notificér ClassService om den nye admin, så centeret kender til instruktøren
        await _classServiceClient.AddAdminToCenter(
            admin.CenterId, admin.FirstName, admin.LastName, admin.Id, admin.Role);

        return admin;
    }

    /// <summary>
    /// Opdaterer adminens data i AdminDB og pusher ændringerne til ClassService.
    /// ID'et fra URL'en bruges altid — forhindrer at klienten kan ændre ID via body.
    /// </summary>
    public async Task<bool> UpdateAsync(string id, Admin updated)
    {
        var existing = await _admins.GetById(id);
        if (existing is null) return false;

        // Tving ID'et fra route-parameteren for at undgå uoverensstemmelse
        updated.Id = id;
        await _admins.Update(id, updated);

        // Opdatér også ClassService, så holdvisningen afspejler navneændringer mv.
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