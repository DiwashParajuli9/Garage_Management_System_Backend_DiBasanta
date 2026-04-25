using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

public class Vendor
{
    public Vendor()
    {
        CreatedAt = DateTime.UtcNow;
        VehicleParts = new List<VehiclePart>();
        PurchaseInvoices = new List<PurchaseInvoice>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ContactPerson { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<VehiclePart> VehicleParts { get; set; }

    public List<PurchaseInvoice> PurchaseInvoices { get; set; }
}