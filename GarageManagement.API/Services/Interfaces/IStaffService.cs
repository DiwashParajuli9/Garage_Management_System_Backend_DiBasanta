using GarageManagement.API.Models.DTOs.Staff;

namespace GarageManagement.API.Services.Interfaces;

/// <summary>
/// Service contract for F2 – Staff Management operations.
/// </summary>
public interface IStaffService
{
    Task<StaffDto> CreateStaffAsync(CreateStaffDto dto);
    Task<List<StaffDto>> GetAllStaffAsync();
    Task<StaffDto> GetStaffByIdAsync(Guid staffId);
    Task<StaffDto> UpdateStaffAsync(Guid staffId, UpdateStaffDto dto);
    Task DeleteStaffAsync(Guid staffId);
}
