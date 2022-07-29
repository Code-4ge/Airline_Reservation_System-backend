﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Admin
{
    [Key]
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Pass { get; set; }
}