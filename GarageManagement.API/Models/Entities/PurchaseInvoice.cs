using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Models.Entities;

public class PurchaseInvoice
{
    public PurchaseInvoice()
    {
        CreatedAt = DateTime.UtcNow;
        Items = new List<PurchaseInvoiceItem>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid VendorId { get; set; }

    [Required]
    public Guid AdminId { get; set; }

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public Vendor Vendor { get; set; } = null!;

    [Required]
    public User Admin { get; set; } = null!;

    [Required]
    public List<PurchaseInvoiceItem> Items { get; set; }
}