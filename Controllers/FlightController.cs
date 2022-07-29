using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class FlightController : ControllerBase
{
    private readonly FlightReservationSystemContext _context;
    
    public FlightController(FlightReservationSystemContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFlightDetails()
    {
        var flights = await _context.FlightDetails.ToListAsync();
        return Ok(flights);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetFlightDetailsById([FromRoute] Guid id)
    {
        var flightDetails = await _context.FlightDetails.FirstOrDefaultAsync(x => x.FlightId == id);

        if (flightDetails != null)
        {
            return Ok(flightDetails);
        }
        return NotFound("Flight Not Found");
    }

    [HttpPost]
    [Route("addflight")]
    public async Task<IActionResult> AddFlightDetails([FromBody] Flight flight)
    {
        if (_context.FlightDetails == null)
        {
            return Problem("Flight Entity is Empty.");
        }
        _context.FlightDetails.Add(flight);
        await _context.SaveChangesAsync();

        return Ok(flight);
    }
    
    [HttpDelete]
    [Route("deleteflight/{id:guid}")]
    public async Task<IActionResult> DeleteFlight([FromRoute] Guid id)
    {
        if (id == null || _context.FlightDetails == null)
        {
            return NotFound();
        }

        var flightDetails = await _context.FlightDetails
            .FirstOrDefaultAsync(m => m.FlightId == id);
        if (flightDetails == null)
        {
            return NotFound();
        }
        _context.Remove(flightDetails);
        await _context.SaveChangesAsync();
        return Ok(flightDetails);
    }
    
    [HttpPut]
    [Route("updateflight/{id:guid}")]
    public async Task<IActionResult> UpdateFlightDetails([FromRoute] Guid id, [FromBody] Flight flight)
    {
        var existingDetails = await _context.FlightDetails.FirstOrDefaultAsync(_ => _.FlightId == id);
        if (existingDetails != null)
        {
            existingDetails.FlightName = flight.FlightName;
            existingDetails.From = flight.From;
            existingDetails.To = flight.To;
            existingDetails.Date = flight.Date;
            existingDetails.Cost = flight.Cost;

            await _context.SaveChangesAsync();
            return Ok("Updated Sucessfully");
        }
        return NotFound("Flight Not Found");
    }

    [HttpGet]
    [Route("totalcost")]
    public async Task<IActionResult> GetFlightTotalCost()
    {
        //var flightsCost = await _context.FlightDetails.FromSqlRaw($"select f.FlightName, sum(convert(int, b.Cost)) as totalCost from FlightDetails as f join BookingDetails as b on f.FlightName = b.FlightName group by f.FlightName").ToListAsync();
        var flightsCost = (from flight in _context.FlightDetails join booking in _context.BookingDetails on flight.FlightName equals booking.FlightName 
                           select new{ flight = flight, booking = booking})
                           .GroupBy(x => new {flightName = x.flight.FlightName})
                           .Select(x => new
                           {
                               flightName = x.First().flight.FlightName,
                               totalCost = x.Sum(y => y.booking.Cost)
                           });
        return Ok(flightsCost);
    }
}