using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.CustomerRegistration;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

/// <summary>
/// Implementation of ICustomerRegistrationService for F6 – Customer Registration.
/// Creates a Customer (User with Role = Customer) and optionally a Vehicle
/// in a single atomic transaction.
/// </summary>
public class CustomerRegistrationService : ICustomerRegistrationService
{
    private readonly AppDbContext _context;

    public CustomerRegistrationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterCustomerResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto)
    {
        // Guard: email must be unique across all users
        var emailInUse = await _context.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (emailInUse)
            throw new Exception($"Email '{dto.Email}' is already registered.");

        // Hash password (BCrypt-style via built-in BCrypt.Net)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var customerId = Guid.NewGuid();

        var customer = new User
        {
            Id           = customerId,
            FullName     = dto.FullName,
            Email        = dto.Email,
            PasswordHash = passwordHash,
            Phone        = dto.Phone,
            Role         = UserRole.Customer,
            CreatedAt    = DateTime.UtcNow
        };

        _context.Users.Add(customer);

        // Optionally attach vehicle
        if (dto.Vehicle is not null)
        {
            var vehicle = new Vehicle
            {
                Id            = Guid.NewGuid(),
                CustomerId    = customerId,
                Make          = dto.Vehicle.Make,
                Model         = dto.Vehicle.Model,
                VehicleNumber = dto.Vehicle.VehicleNumber,
                Year          = dto.Vehicle.Year,
                CreatedAt     = DateTime.UtcNow
            };
            _context.Vehicles.Add(vehicle);
        }

        await _context.SaveChangesAsync();

        // Reload with vehicles for response
        var created = await _context.Users
            .Include(u => u.Vehicles)
            .FirstAsync(u => u.Id == customerId);

        return new RegisterCustomerResponseDto
        {
            CustomerId   = created.Id,
            FullName     = created.FullName,
            Email        = created.Email,
            Phone        = created.Phone,
            RegisteredAt = created.CreatedAt,
            Vehicles     = created.Vehicles.Select(v => new RegisteredVehicleDto
            {
                Id            = v.Id,
                Make          = v.Make,
                Model         = v.Model,
                VehicleNumber = v.VehicleNumber,
                Year          = v.Year
            }).ToList()
        };
    }
}
