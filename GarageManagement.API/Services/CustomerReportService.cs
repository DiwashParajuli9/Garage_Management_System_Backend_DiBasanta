using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Reports;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

/// <summary>
/// Implementation of ICustomerReportService for F9 – Customer Reports.
/// </summary>
public class CustomerReportService : ICustomerReportService
{
    private readonly AppDbContext _context;

    public CustomerReportService(AppDbContext context)
    {
        _context = context;
    }

    // ── Report 1: Total customers ───────────────────────────────────────────

    public async Task<TotalCustomersDto> GetTotalCustomersAsync()
    {
        var count = await _context.Users
            .CountAsync(u => u.Role == UserRole.Customer);

        return new TotalCustomersDto { TotalCustomers = count };
    }

    // ── Report 2: Customers grouped by vehicle make ─────────────────────────

    public async Task<List<CustomersByVehicleDto>> GetCustomersByVehicleAsync()
    {
        // Join Vehicles → Users (Customer role), group by Make
        var result = await _context.Vehicles
            .Where(v => v.Customer.Role == UserRole.Customer)
            .GroupBy(v => v.Make)
            .Select(g => new CustomersByVehicleDto
            {
                VehicleMake   = g.Key,
                // Distinct customers who own a vehicle of this make
                CustomerCount = g.Select(v => v.CustomerId).Distinct().Count()
            })
            .OrderByDescending(r => r.CustomerCount)
            .ToListAsync();

        return result;
    }

    // ── Report 3: Most recently registered customers ────────────────────────

    public async Task<List<RecentCustomerDto>> GetRecentCustomersAsync(int count = 10)
    {
        var customers = await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .Include(u => u.Vehicles)
            .OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .Select(u => new RecentCustomerDto
            {
                CustomerId   = u.Id,
                FullName     = u.FullName,
                Email        = u.Email,
                Phone        = u.Phone,
                RegisteredAt = u.CreatedAt,
                VehicleCount = u.Vehicles.Count
            })
            .ToListAsync();

        return customers;
    }
}
