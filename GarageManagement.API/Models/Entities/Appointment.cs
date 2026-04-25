using System.ComponentModel.DataAnnotations;
using GarageManagement.API.Models.Entities.Enums;

namespace GarageManagement.API.Models.Entities;

public class Appointment
{
    public Appointment()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public DateTime AppointmentDate { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public AppointmentStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public User Customer { get; set; } = null!;
}