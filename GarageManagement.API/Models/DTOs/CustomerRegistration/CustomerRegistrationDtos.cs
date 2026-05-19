using System.ComponentModel.DataAnnotations;

namespace GarageManagement.API.Models.DTOs.CustomerRegistration;

// ── Vehicle sub-DTO ─────────────────────────────────────────────────────────

/// <summary>Vehicle details included in the registration request.</summary>
public class RegisterVehicleDto
{
    [Required]
    [MaxLength(100)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [Range(1886, 9999)]
    public int Year { get; set; }
}

// ── Registration request ────────────────────────────────────────────────────

/// <summary>
/// F6 – Register a new customer together with their vehicle in one request.
/// </summary>
public class RegisterCustomerDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Optional: vehicle to register at the same time.
    /// If omitted the customer is created without a vehicle.
    /// </summary>
    public RegisterVehicleDto? Vehicle { get; set; }
}

// ── Registration response ───────────────────────────────────────────────────

public class RegisteredVehicleDto
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class RegisterCustomerResponseDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public List<RegisteredVehicleDto> Vehicles { get; set; } = new();
}
