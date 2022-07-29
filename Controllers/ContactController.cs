using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ContactController : ControllerBase
{
    private readonly FlightReservationSystemContext _context;

    public ContactController(FlightReservationSystemContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("addmessage")]
    public async Task<IActionResult> RegisterAdmin([FromBody] Contact message)
    {
        if (_context.ContactDetails == null)
        {
            return Problem("Contact Entity is Empty.");
        }
        _context.ContactDetails.Add(message);
        await _context.SaveChangesAsync();

        return Ok(message);
    }
}
