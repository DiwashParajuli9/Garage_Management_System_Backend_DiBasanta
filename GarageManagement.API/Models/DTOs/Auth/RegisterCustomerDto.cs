namespace GarageManagement.API.Models.DTOs.Auth;

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