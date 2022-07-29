using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BookingController : ControllerBase
{
    private readonly FlightReservationSystemContext _context;

    public BookingController(FlightReservationSystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetBookingFlight([FromRoute] Guid id)
    {
        var bookedflights = await _context.BookingDetails.Where(m => m.UserId == id).ToListAsync();
        if (bookedflights == null)
        {
            return NotFound();
        }
        return Ok(bookedflights);
    }

    [HttpPost]
    [Route("bookflight")]
    public async Task<IActionResult> BookFlight([FromBody] Booking booked)
    {
        if (_context.BookingDetails == null)
        {
            return Problem("BookedFlight Entity is Empty.");
        }
        _context.BookingDetails.Add(booked);
        await _context.SaveChangesAsync();

        return Ok(booked);
    }

    [HttpDelete]
    [Route("deleteBookedFlight/{id:guid}")]
    public async Task<IActionResult> DeleteBookedFlight([FromRoute] Guid id)
    {
        if (id == null || _context.BookingDetails == null)
        {
            return NotFound();
        }

        var bookedFlightDetails = await _context.BookingDetails
            .FirstOrDefaultAsync(m => m.BookedId == id);
        if (bookedFlightDetails == null)
        {
            return NotFound();
        }
        _context.Remove(bookedFlightDetails);
        await _context.SaveChangesAsync();
        return Ok(bookedFlightDetails);
    }
}
