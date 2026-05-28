using AdminService.Models;
using MongoDB.Driver;

namespace AdminService.Repositories;

/// <summary>
/// MongoDB-implementation af <see cref="IAdminRepository"/>.
/// Alle operationer går direkte mod <c>AdminCollection</c> i AdminDB.
/// </summary>
public class AdminRepository : IAdminRepository
{
    private readonly IMongoCollection<Admin> _adminCollection;

    /// <summary>
    /// <paramref name="db"/> injiceres fra Program.cs, hvor databaseforbindelsen konfigureres.
    /// </summary>
    public AdminRepository(IMongoDatabase db)
    {
        _adminCollection = db.GetCollection<Admin>("AdminCollection");
    }

    /// <summary>Henter alle admins — bruges bl.a. af frontend til at vise medarbejderlisten.</summary>
    public async Task<List<Admin>> GetAll()
    {
        // Find(_ => true) er MongoDB-ækvivalenten til SELECT * — ingen filtrering
        return await _adminCollection.Find(_ => true).ToListAsync();
    }

    public async Task<List<Admin>> GetByCenterAsync(string centerId)
    {
        return await _adminCollection
            .Find(a => a.CenterId == centerId)
            .ToListAsync();
    }

    public async Task<Admin?> GetById(string id)
    {
        return await _adminCollection
            .Find(a => a.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task Create(Admin admin)
    {
        await _adminCollection.InsertOneAsync(admin);
    }

    /// <summary>
    /// Erstatter hele dokumentet (ReplaceOne) frem for at opdatere enkeltfelter.
    /// Simpelt og robust, men kræver at hele admin-objektet sendes med.
    /// </summary>
    public async Task Update(string id, Admin admin)
    {
        await _adminCollection.ReplaceOneAsync(a => a.Id == id, admin);
    }

    public async Task Delete(string id)
    {
        await _adminCollection.DeleteOneAsync(a => a.Id == id);
    }
}