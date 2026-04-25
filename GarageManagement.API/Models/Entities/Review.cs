using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

public class Review
{
    public Review()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [Required]
    public User Customer { get; set; } = null!;
}