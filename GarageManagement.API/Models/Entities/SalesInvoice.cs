using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Models.Entities;

public class SalesInvoice
{
    public SalesInvoice()
    {
        CreatedAt = DateTime.UtcNow;
        Items = new List<SalesInvoiceItem>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public Guid StaffId { get; set; }

    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    public bool DiscountApplied { get; set; }

    [Precision(18, 2)]
    public decimal FinalAmount { get; set; }

    public bool IsPaid { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public User Customer { get; set; } = null!;

    [Required]
    public User Staff { get; set; } = null!;

    [Required]
    public List<SalesInvoiceItem> Items { get; set; }
}