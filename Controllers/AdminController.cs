using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AdminController : ControllerBase
{
    
    private readonly FlightReservationSystemContext _context;
    
    public AdminController(FlightReservationSystemContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAdminDetails()
    {
        var admin = await _context.AdminDetails.ToListAsync();
        return Ok(admin);

    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] Admin admin)
    {
        if (_context.AdminDetails == null)
        {
            return Problem("Admin Entity is Empty.");
        }
        _context.AdminDetails.Add(admin);
        await _context.SaveChangesAsync();

        return Ok(admin);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<Admin>> Login([FromBody] Admin admin)
    {
        if (_context.AdminDetails == null)
        {
            return NotFound("Invalid credentials not found");
        }
        var A_Details = await _context.AdminDetails
            .Where(x => x.Email == admin.Email && x.Pass == admin.Pass)
            .Select(x => new Admin()
            {
                Id = x.Id,
                Email = x.Email,
                Pass = x.Pass,

            })
            .FirstOrDefaultAsync();

        if (A_Details == null)
        {
            return NotFound("Invalid credentials not found");
        }
        return Ok(A_Details);
    }
}