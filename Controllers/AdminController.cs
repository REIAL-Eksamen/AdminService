using AdminService.DTOs;
using Microsoft.AspNetCore.Mvc;
using AdminService.Models;
using AdminService.Clients;
using AdminService.Repositories;


[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly IClassServiceClient _classServiceClient;
    private readonly IAdminRepository _repository;
    private readonly ILogger<AdminController> _logger;

  /*  public AdminController(IMongoClient mongoClient, ClassServiceClient classServiceClient, ILogger<AdminController> logger)
    {
        var database = mongoClient.GetDatabase("AdminDB");
        _adminCollection = database.GetCollection<Admin>("AdminCollection");
        _classServiceClient = classServiceClient;
        _logger = logger;
    }
    */
  
  public AdminController(
      IAdminRepository repository,
      IClassServiceClient classServiceClient,
      ILogger<AdminController> logger)
  {
      _repository = repository;
      _classServiceClient = classServiceClient;
      _logger = logger;
  }
  
  
    // Henter alle admins
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _repository.GetAll();
        return Ok(admins);
    }

    // Henter en specifik admin via ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(string id)
    {
        var admin = await _repository.GetById(id);
        if (admin == null) return NotFound();
        return Ok(admin);
    }

    // Opretter en ny admin og tilknytter den til et center i ClassService
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin admin)
    {
        await _repository.Create(admin);
        await _classServiceClient.AddAdminToCenter(
            admin.CenterId,
            admin.FirstName,
            admin.LastName,
            admin.Id,
            admin.Role);       
        return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, admin);
    }

    // Opdaterer en eksisterende admin via ID
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(string id, Admin updated)
    {
        var existing = await _repository.GetById(id);
        if (existing == null) return NotFound();

        updated.Id = id;
        await _repository.Update(id, updated);
        await _classServiceClient.AddAdminToCenter(
            updated.CenterId,
            updated.FirstName,
            updated.LastName,
            updated.Id,
            updated.Role);
        return NoContent();
    }

    // Sletter en admin via ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(string id)
    {
        await _repository.Delete(id);
        return NoContent();
    }
}