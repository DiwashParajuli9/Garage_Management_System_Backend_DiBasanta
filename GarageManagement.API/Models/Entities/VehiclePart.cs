using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Models.Entities;

public class VehiclePart
{
    public VehiclePart()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public Guid VendorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Required]
    public Vendor Vendor { get; set; } = null!;
}