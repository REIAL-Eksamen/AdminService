using AdminService.Models;
using MongoDB.Driver;

namespace AdminService.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly IMongoCollection<Admin> _adminCollection;

    public AdminRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("AdminDB");
        _adminCollection = database.GetCollection<Admin>("AdminCollection");
    }

    public async Task<List<Admin>> GetAll()
    {
        return await _adminCollection.Find(_ => true).ToListAsync();
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

    public async Task Update(string id, Admin admin)
    {
        await _adminCollection.ReplaceOneAsync(a => a.Id == id, admin);
    }

    public async Task Delete(string id)
    {
        await _adminCollection.DeleteOneAsync(a => a.Id == id);
    }
}