using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Vendor;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageManagement.API.Services
{
    public class VendorService : IVendorService
    {
        private readonly AppDbContext _context;

        public VendorService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
        {
            return await _context.Vendors
                .Select(v => new VendorDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    ContactPerson = v.ContactPerson,
                    Phone = v.Phone,
                    Email = v.Email,
                    Address = v.Address
                }).ToListAsync();
        }

        public async Task<VendorDto> GetVendorByIdAsync(Guid id)
        {
            var v = await _context.Vendors.FindAsync(id);
            if (v == null) return null;
            return new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                ContactPerson = v.ContactPerson,
                Phone = v.Phone,
                Email = v.Email,
                Address = v.Address
            };
        }

        public async Task<VendorDto> CreateVendorAsync(CreateVendorDto dto)
        {
            var existingVendor = await _context.Vendors
                .AnyAsync(v => v.Email == dto.Email || v.Phone == dto.Phone);

            if (existingVendor)
            {
                throw new Exception("A vendor with the same email or phone already exists.");
            }

            var vendor = new Vendor
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return MapToDto(vendor);
        }

        public async Task<bool> UpdateVendorAsync(Guid id, UpdateVendorDto dto)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return false;

            var duplicate = await _context.Vendors
                .AnyAsync(v => (v.Email == dto.Email || v.Phone == dto.Phone) && v.Id != id);

            if (duplicate)
            {
                throw new Exception("Another vendor with the same email or phone already exists.");
            }

            vendor.Name = dto.Name;
            vendor.ContactPerson = dto.ContactPerson;
            vendor.Phone = dto.Phone;
            vendor.Email = dto.Email;
            vendor.Address = dto.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        private static VendorDto MapToDto(Vendor v)
        {
            return new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                ContactPerson = v.ContactPerson,
                Phone = v.Phone,
                Email = v.Email,
                Address = v.Address
            };
        }

        public async Task<bool> DeleteVendorAsync(Guid id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return false;
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}