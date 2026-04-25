using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Customer;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerProfileDto> GetMyProfile(Guid customerId)
    {
        var customer = await _context.Users
            .Include(user => user.Vehicles)
            .FirstOrDefaultAsync(user => user.Id == customerId);

        if (customer is null)
        {
            throw new Exception("Customer not found");
        }

        return MapCustomerProfile(customer);
    }

    public async Task<CustomerProfileDto> UpdateMyProfile(Guid customerId, UpdateProfileDto dto)
    {
        var customer = await _context.Users.FirstOrDefaultAsync(user => user.Id == customerId);
        if (customer is null)
        {
            throw new Exception("Customer not found");
        }

        customer.FullName = dto.FullName;
        customer.Phone = dto.Phone;
        await _context.SaveChangesAsync();

        return await GetMyProfile(customerId);
    }

    public async Task<VehicleDto> AddVehicle(Guid customerId, AddVehicleDto dto)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Make = dto.Make,
            Model = dto.Model,
            VehicleNumber = dto.VehicleNumber,
            Year = dto.Year
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return MapVehicle(vehicle);
    }

    public async Task<VehicleDto> UpdateVehicle(Guid customerId, Guid vehicleId, UpdateVehicleDto dto)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(item => item.Id == vehicleId && item.CustomerId == customerId);

        if (vehicle is null)
        {
            throw new Exception("Vehicle not found");
        }

        vehicle.Make = dto.Make;
        vehicle.Model = dto.Model;
        vehicle.VehicleNumber = dto.VehicleNumber;
        vehicle.Year = dto.Year;

        await _context.SaveChangesAsync();
        return MapVehicle(vehicle);
    }

    public async Task DeleteVehicle(Guid customerId, Guid vehicleId)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(item => item.Id == vehicleId && item.CustomerId == customerId);

        if (vehicle is null)
        {
            throw new Exception("Vehicle not found");
        }

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<List<VehicleDto>> GetMyVehicles(Guid customerId)
    {
        return await _context.Vehicles
            .Where(vehicle => vehicle.CustomerId == customerId)
            .Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                VehicleNumber = vehicle.VehicleNumber
            })
            .ToListAsync();
    }

    private static CustomerProfileDto MapCustomerProfile(User customer)
    {
        return new CustomerProfileDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            CreatedAt = customer.CreatedAt,
            Vehicles = customer.Vehicles.Select(MapVehicle).ToList()
        };
    }

    private static VehicleDto MapVehicle(Vehicle vehicle)
    {
        return new VehicleDto
        {
            Id = vehicle.Id,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            VehicleNumber = vehicle.VehicleNumber
        };
    }
}