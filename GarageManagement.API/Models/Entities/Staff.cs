using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.Entities;

/// <summary>
/// Represents a garage staff member (F2 – Staff Management).
/// </summary>
public class Staff
{
    public Staff()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
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
    [Phone]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Job role / position title (e.g. Mechanic, Receptionist).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Whether the staff member is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
