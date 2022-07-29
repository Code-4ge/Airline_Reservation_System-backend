using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Contact
{
    [Key]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Msg { get; set; }
}
