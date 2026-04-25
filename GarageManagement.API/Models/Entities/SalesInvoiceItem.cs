using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Models.Entities;

public class SalesInvoiceItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid SalesInvoiceId { get; set; }

    [Required]
    public Guid VehiclePartId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    [Required]
    public SalesInvoice SalesInvoice { get; set; } = null!;

    [Required]
    public VehiclePart VehiclePart { get; set; } = null!;
}