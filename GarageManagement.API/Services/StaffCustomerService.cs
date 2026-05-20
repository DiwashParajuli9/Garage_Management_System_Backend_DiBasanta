using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Customer;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class StaffCustomerService : IStaffCustomerService
{
    private readonly AppDbContext _context;

    public StaffCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StaffCustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .Include(u => u.Vehicles)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => MapToDto(u))
            .ToListAsync();

        return customers;
    }

    public async Task<StaffCustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await _context.Users
            .Where(u => u.Id == customerId && u.Role == UserRole.Customer)
            .Include(u => u.Vehicles)
            .FirstOrDefaultAsync();

        if (customer is null)
            throw new Exception("Customer not found");

        return MapToDto(customer);
    }

    public async Task<List<CustomerSearchDto>> SearchCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<CustomerSearchDto>();

        var search = searchTerm.ToLower();
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer &&
                   (u.FullName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.Phone.Contains(search)))
            .Select(u => new CustomerSearchDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone
            })
            .Take(20)
            .ToListAsync();

        return customers;
    }

    public async Task<List<CustomerServiceHistoryDto>> GetCustomerServiceHistoryAsync(Guid customerId)
    {
        var history = await _context.SalesInvoices
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new CustomerServiceHistoryDto
            {
                InvoiceId = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                ServiceDate = i.CreatedAt,
                ServiceType = "Service", // TODO: Add service type tracking if needed
                Amount = i.GrandTotal,
                Status = i.PaymentStatus
            })
            .ToListAsync();

        return history;
    }

    public async Task<StaffCustomerDto> CreateCustomerAsync(CreateStaffCustomerDto dto)
    {
        // Check if customer already exists with same email
        var existingCustomer = await _context.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (existingCustomer)
            throw new Exception($"Customer with email '{dto.Email}' already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()[..10]),
            Role = UserRole.Customer
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Add vehicles if provided
        if (dto.Vehicles.Any())
        {
            foreach (var vehicleDto in dto.Vehicles)
            {
                var vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    CustomerId = user.Id,
                    Make = vehicleDto.Make,
                    Model = vehicleDto.Model,
                    VehicleNumber = vehicleDto.VehicleNumber,
                    Year = vehicleDto.Year
                };
                _context.Vehicles.Add(vehicle);
            }
            await _context.SaveChangesAsync();
        }

        return await GetCustomerByIdAsync(user.Id);
    }

    public async Task<StaffCustomerDto> UpdateCustomerAsync(Guid customerId, UpdateStaffCustomerDto dto)
    {
        var customer = await _context.Users
            .Where(u => u.Id == customerId && u.Role == UserRole.Customer)
            .FirstOrDefaultAsync();

        if (customer is null)
            throw new Exception("Customer not found");

        customer.FullName = dto.FullName;
        customer.Phone = dto.Phone;

        _context.Users.Update(customer);
        await _context.SaveChangesAsync();

        return await GetCustomerByIdAsync(customerId);
    }

    public async Task DeleteCustomerAsync(Guid customerId)
    {
        var customer = await _context.Users
            .Where(u => u.Id == customerId && u.Role == UserRole.Customer)
            .FirstOrDefaultAsync();

        if (customer is null)
            throw new Exception("Customer not found");

        _context.Users.Remove(customer);
        await _context.SaveChangesAsync();
    }

    private StaffCustomerDto MapToDto(User customer)
    {
        var invoices = _context.SalesInvoices
            .Where(i => i.CustomerId == customer.Id)
            .ToList();

        var totalSpend = invoices.Sum(i => i.GrandTotal);
        var creditRemaining = invoices
            .Where(i => i.PaymentStatus == "Credit")
            .Sum(i => i.GrandTotal);

        return new StaffCustomerDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.FullName, // TODO: Add address field to User entity
            RegisteredAt = customer.CreatedAt,
            LastVisit = customer.UpdatedAt,
            TotalSpend = totalSpend,
            CreditBalance = creditRemaining,
            Vehicles = customer.Vehicles?.Select(v => new StaffVehicleDto
            {
                Id = v.Id,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                VehicleNumber = v.VehicleNumber,
                Color = "", // TODO: Add color field to Vehicle entity
                FuelType = "", // TODO: Add fuelType field to Vehicle entity
                Vin = "" // TODO: Add VIN field to Vehicle entity
            }).ToList() ?? new List<StaffVehicleDto>()
        };
    }
}
