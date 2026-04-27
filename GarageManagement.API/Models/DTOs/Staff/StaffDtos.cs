using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.DTOs.Staff;

// ── Request DTOs ────────────────────────────────────────────────────────────

/// <summary>DTO used when creating a new staff member.</summary>
public class CreateStaffDto
{
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

    [Required]
    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;
}

/// <summary>DTO used when updating an existing staff member.</summary>
public class UpdateStaffDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

// ── Response DTO ────────────────────────────────────────────────────────────

/// <summary>Staff member data returned from the API.</summary>
public class StaffDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
