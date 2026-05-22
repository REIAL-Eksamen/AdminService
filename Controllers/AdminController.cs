using AdminService.DTOs;
using Microsoft.AspNetCore.Mvc;
using AdminService.Models;
using AdminService.Clients;
using MongoDB.Driver;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMongoCollection<Admin> _adminCollection;
    private readonly ClassServiceClient _classServiceClient;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IMongoClient mongoClient, ClassServiceClient classServiceClient, ILogger<AdminController> logger)
    {
        var database = mongoClient.GetDatabase("AdminDB");
        _adminCollection = database.GetCollection<Admin>("AdminCollection");
        _classServiceClient = classServiceClient;
        _logger = logger;
    }

    // Henter alle admins
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _adminCollection.Find(_ => true).ToListAsync();
        return Ok(admins);
    }

    // Henter en specifik admin via ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(string id)
    {
        var admin = await _adminCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        if (admin == null) return NotFound();
        return Ok(admin);
    }

    // Opretter en ny admin og tilknytter den til et center i ClassService
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin admin)
    {
        await _adminCollection.InsertOneAsync(admin);
        await _classServiceClient.AddAdminToCenter(admin.CenterId, admin.Id, admin.FirstName, admin.LastName, admin.Role);
        return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, admin);
    }

    // Opdaterer en eksisterende admin via ID
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(string id, Admin updated)
    {
        var existing = await _adminCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        if (existing == null) return NotFound();

        updated.Id = id;
        await _adminCollection.ReplaceOneAsync(a => a.Id == id, updated);
        await _classServiceClient.AddAdminToCenter(updated.CenterId, updated.Id, updated.FirstName, updated.LastName, updated.Role);
        return NoContent();
    }

    // Sletter en admin via ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(string id)
    {
        var result = await _adminCollection.DeleteOneAsync(a => a.Id == id);
        if (result.DeletedCount == 0) return NotFound();
        return NoContent();
    }
}