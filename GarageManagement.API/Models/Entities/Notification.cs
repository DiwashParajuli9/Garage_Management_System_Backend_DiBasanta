using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

public class Notification
{
    public Notification()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    public User User { get; set; } = null!;
}