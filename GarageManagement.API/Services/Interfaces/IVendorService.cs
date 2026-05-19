using GarageManagement.API.Models.DTOs.Vendor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarageManagement.API.Services.Interfaces
{
    public interface IVendorService
    {
    Task<IEnumerable<VendorDto>> GetAllVendorsAsync();
    Task<VendorDto> GetVendorByIdAsync(Guid id);
    Task<VendorDto> CreateVendorAsync(CreateVendorDto dto);
    Task<bool> UpdateVendorAsync(Guid id, UpdateVendorDto dto);
    Task<bool> DeleteVendorAsync(Guid id);
    }
}