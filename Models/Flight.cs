using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Flight
{
    [Key]
    public Guid FlightId { get; set; }
    public string? FlightName { get; set;}
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public int? Cost { get; set; }
}