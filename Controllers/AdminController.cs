using Microsoft.AspNetCore.Mvc;
using AdminService.Models;

namespace AdminService.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private static readonly List<Admin> Admins = new()
    {
        new Admin
        {
            AdminId = 1,
            FirstName = "James",
            LastName = "Bond",
            Email = "jb@007.uk",
            Password = "123456",
            Role = "Instructor"
        }
    };
    
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IEnumerable<Admin> Get()
    {
        return Admins;
    }

    [HttpGet("{adminId}", Name = "GetAdminById")]
    public ActionResult<Admin> GetAdminById(int adminId)
    {
        var admin = Admins.FirstOrDefault(u => u.AdminId == adminId);
        
        if (admin == null)
        {
            return NotFound();
        }
        
        return admin;
    }
    
    
}