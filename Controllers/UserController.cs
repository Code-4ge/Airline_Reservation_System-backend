using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class UserController : ControllerBase
{
    private readonly FlightReservationSystemContext _context;

    public UserController(FlightReservationSystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserDetails()
    {
        var users = await _context.UsersDetails.ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        if (_context.UsersDetails == null)
        {
            return Problem("User Entity is Empty.");
        }
        _context.UsersDetails.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<Admin>> LoginUser([FromBody] User user)
    {
        if (_context.UsersDetails == null)
        {
            return NotFound("Invalid credentials not found");
        }
        var regUser = await _context.UsersDetails
            .Where(x => x.Email == user.Email && x.Pass == user.Pass)
            .Select(x => new User()
            {
                Id = x.Id,
                Email = x.Email,
                Pass = x.Pass,

            })
            .FirstOrDefaultAsync();

        if (regUser == null)
        {
            return NotFound("Invalid credentials not found");
        }
        return Ok(regUser);
    }
}

