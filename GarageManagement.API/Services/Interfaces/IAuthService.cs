using GarageManagement.API.Models.DTOs.Auth;

namespace GarageManagement.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> RegisterCustomer(RegisterCustomerDto dto);
    Task<LoginResponseDto> RegisterStaff(RegisterStaffDto dto);
    Task<LoginResponseDto> Login(LoginDto dto);
    Task<List<UserProfileDto>> GetAllStaff();
    Task DeleteStaff(Guid staffId);
}