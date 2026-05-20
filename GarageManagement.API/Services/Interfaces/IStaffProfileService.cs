using GarageManagement.API.Models.DTOs.Staff;

namespace GarageManagement.API.Services.Interfaces;

public interface IStaffProfileService
{
    Task<StaffProfileDto> GetProfileByIdAsync(Guid staffId);
    Task<StaffProfileDto> GetMyProfileAsync(Guid staffId);
    Task<List<StaffSalesHistoryDto>> GetMyProfileSalesHistoryAsync(Guid staffId);
    Task<StaffProfileDto> UpdateProfileAsync(Guid staffId, UpdateStaffProfileDto dto);
}
