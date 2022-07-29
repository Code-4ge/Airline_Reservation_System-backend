using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class FlightReservationSystemContext : DbContext
{
    public FlightReservationSystemContext(DbContextOptions<FlightReservationSystemContext> options) : base(options)
    {

    }
    
    public DbSet<Flight>? FlightDetails { get; set; }
    
    public DbSet<Admin>? AdminDetails { get; set; }

    public DbSet<User>? UsersDetails { get; set; }

    public DbSet<Booking>? BookingDetails { get; set; }

    public DbSet<Contact>? ContactDetails { get; set; }


}