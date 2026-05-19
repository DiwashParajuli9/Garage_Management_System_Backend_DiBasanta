using GarageManagement.API.Models.DTOs.Customer;

namespace GarageManagement.API.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerProfileDto> GetMyProfile(Guid customerId);
    Task<CustomerProfileDto> UpdateMyProfile(Guid customerId, UpdateProfileDto dto);
    Task<VehicleDto> AddVehicle(Guid customerId, AddVehicleDto dto);
    Task<VehicleDto> UpdateVehicle(Guid customerId, Guid vehicleId, UpdateVehicleDto dto);
    Task DeleteVehicle(Guid customerId, Guid vehicleId);
    Task<List<VehicleDto>> GetMyVehicles(Guid customerId);
    Task<List<InvoiceHistoryDto>> GetMyHistory(Guid customerId);
}