using GarageManagement.API.Models.DTOs.CustomerRegistration;

namespace GarageManagement.API.Services.Interfaces;

/// <summary>
/// Service contract for F6 – Customer Registration.
/// </summary>
public interface ICustomerRegistrationService
{
    Task<RegisterCustomerResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto);
}
