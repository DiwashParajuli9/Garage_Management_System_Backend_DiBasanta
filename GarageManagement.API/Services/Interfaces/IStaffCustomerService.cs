using GarageManagement.API.Models.DTOs.Customer;

namespace GarageManagement.API.Services.Interfaces;

public interface IStaffCustomerService
{
    Task<List<StaffCustomerDto>> GetAllCustomersAsync();
    Task<StaffCustomerDto> GetCustomerByIdAsync(Guid customerId);
    Task<List<CustomerSearchDto>> SearchCustomersAsync(string searchTerm);
    Task<List<CustomerServiceHistoryDto>> GetCustomerServiceHistoryAsync(Guid customerId);
    Task<StaffCustomerDto> CreateCustomerAsync(CreateStaffCustomerDto dto);
    Task<StaffCustomerDto> UpdateCustomerAsync(Guid customerId, UpdateStaffCustomerDto dto);
    Task DeleteCustomerAsync(Guid customerId);
}
