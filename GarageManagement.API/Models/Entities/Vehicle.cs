using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

public class Vehicle
{
    public Vehicle()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [Range(1886, 9999)]
    public int Year { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public User Customer { get; set; } = null!;
}