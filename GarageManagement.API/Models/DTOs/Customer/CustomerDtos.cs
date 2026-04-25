namespace GarageManagement.API.Models.DTOs.Customer;

public class RegisterCustomerDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public int VehicleYear { get; set; }
}

public class UpdateProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class AddVehicleDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class UpdateVehicleDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class VehicleDto
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
}

public class CustomerProfileDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<VehicleDto> Vehicles { get; set; } = new();
}