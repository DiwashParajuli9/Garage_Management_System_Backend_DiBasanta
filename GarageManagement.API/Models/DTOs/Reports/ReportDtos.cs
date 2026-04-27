namespace GarageManagement.API.Models.DTOs.Reports;

// ── F9 Report Response DTOs ─────────────────────────────────────────────────

/// <summary>Total customer count.</summary>
public class TotalCustomersDto
{
    public int TotalCustomers { get; set; }
}

/// <summary>Customers grouped by vehicle make.</summary>
public class CustomersByVehicleDto
{
    public string VehicleMake { get; set; } = string.Empty;
    public int CustomerCount { get; set; }
}

/// <summary>Summary of a recently registered customer.</summary>
public class RecentCustomerDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public int VehicleCount { get; set; }
}
