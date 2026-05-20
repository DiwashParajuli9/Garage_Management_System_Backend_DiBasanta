namespace GarageManagement.API.Models.DTOs.Customer;

/// <summary>Staff view of customer with full details, vehicles, and history</summary>
public class StaffCustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public DateTime? LastVisit { get; set; }
    public decimal TotalSpend { get; set; }
    public decimal CreditBalance { get; set; }
    public List<StaffVehicleDto> Vehicles { get; set; } = new();
}

public class StaffVehicleDto
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
}

public class CreateStaffCustomerDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<CreateVehicleDto> Vehicles { get; set; } = new();
}

public class UpdateStaffCustomerDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class CustomerSearchDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class CustomerServiceHistoryDto
{
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime ServiceDate { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}
