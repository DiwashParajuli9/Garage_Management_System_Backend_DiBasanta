using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

public class User
{
    public User()
    {
        CreatedAt = DateTime.UtcNow;
        Vehicles = new List<Vehicle>();
        CustomerSalesInvoices = new List<SalesInvoice>();
        StaffSalesInvoices = new List<SalesInvoice>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public Models.Entities.Enums.UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Vehicle> Vehicles { get; set; }

    public List<SalesInvoice> CustomerSalesInvoices { get; set; }

    public List<SalesInvoice> StaffSalesInvoices { get; set; }
}