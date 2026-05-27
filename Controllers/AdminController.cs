using AdminService.Models;
using AdminService.Services;
using AdminService.Repositories;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _adminService.GetAllAsync();
        return Ok(admins);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(string id)
    {
        var admin = await _adminService.GetByIdAsync(id);
        if (admin is null) return NotFound();
        return Ok(admin);
    }
    
    [HttpGet("bycenter/{centerId}")]
    public async Task<IActionResult> GetByCenter(string centerId)
    {
        var admins = await _adminService.GetByCenterAsync(centerId);
        return Ok(admins);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin admin)
    {
        var created = await _adminService.CreateAsync(admin);
        return CreatedAtAction(nameof(GetAdmin), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(string id, Admin updated)
    {
        var success = await _adminService.UpdateAsync(id, updated);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(string id)
    {
        var deleted = await _adminService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}