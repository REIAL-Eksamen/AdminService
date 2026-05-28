using AdminService.Models;
using AdminService.Services;
using AdminService.Repositories;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// REST API-controller til administration af admins (medarbejdere).
/// Eksponerer CRUD-operationer og videresender kald til <see cref="IAdminService"/>.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminService adminService, ILogger<AdminController> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    /// <summary>Henter alle admins på tværs af alle centre.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _adminService.GetAllAsync();
        return Ok(admins);
    }

    /// <summary>Henter én admin ud fra MongoDB-ID.</summary>
    /// <response code="404">Returneres hvis adminen ikke eksisterer.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(string id)
    {
        var admin = await _adminService.GetByIdAsync(id);
        if (admin is null) return NotFound();
        return Ok(admin);
    }

    /// <summary>Henter alle admins tilknyttet et bestemt center.</summary>
    [HttpGet("bycenter/{centerId}")]
    public async Task<IActionResult> GetByCenter(string centerId)
    {
        var admins = await _adminService.GetByCenterAsync(centerId);
        return Ok(admins);
    }

    /// <summary>
    /// Opretter en ny admin og synkroniserer til ClassService.
    /// Returnerer 201 Created med den oprettede admin i body.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin admin)
    {
        var created = await _adminService.CreateAsync(admin);
        return CreatedAtAction(nameof(GetAdmin), new { id = created.Id }, created);
    }

    /// <summary>
    /// Opdaterer en eksisterende admin og synkroniserer ændringerne til ClassService.
    /// </summary>
    /// <response code="204">Opdatering lykkedes.</response>
    /// <response code="404">Adminen findes ikke.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(string id, Admin updated)
    {
        var success = await _adminService.UpdateAsync(id, updated);
        if (!success) return NotFound();
        return NoContent();
    }

    /// <summary>Sletter en admin permanent fra AdminDB.</summary>
    /// <response code="204">Sletning lykkedes.</response>
    /// <response code="404">Adminen findes ikke.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(string id)
    {
        var deleted = await _adminService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}