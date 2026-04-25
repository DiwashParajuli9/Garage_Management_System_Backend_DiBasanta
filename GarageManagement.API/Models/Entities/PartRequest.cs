using System.ComponentModel.DataAnnotations;
using GarageManagement.API.Models.Entities.Enums;

namespace GarageManagement.API.Models.Entities;

public class PartRequest
{
    public PartRequest()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [MaxLength(200)]
    public string PartName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public PartRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public User Customer { get; set; } = null!;
}