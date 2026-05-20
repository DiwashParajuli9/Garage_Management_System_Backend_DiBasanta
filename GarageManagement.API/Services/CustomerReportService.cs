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
feature/diwash-F1-F7-F12-F15

    // ── Report 4: Customer service history ──────────────────────────────────

    public async Task<List<dynamic>> GetCustomerServiceHistoryAsync(Guid? customerId = null)
    {
        var query = _context.SalesInvoices.AsQueryable();

        if (customerId.HasValue)
        {
            query = query.Where(i => i.CustomerId == customerId.Value);
        }

        var history = await query
            .Include(i => i.Customer)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new
            {
                InvoiceId = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer.FullName,
                ServiceDate = i.CreatedAt,
                Amount = i.FinalAmount,
                PaymentStatus = i.PaymentStatus,
                ItemCount = i.Items.Count
            })
            .Cast<dynamic>()
            .ToListAsync();

        return history;
    }

    // ── Report 5: Customer sales invoices ───────────────────────────────────

    public async Task<List<dynamic>> GetCustomerSalesInvoicesAsync(Guid? customerId = null)
    {
        var query = _context.SalesInvoices.AsQueryable();

        if (customerId.HasValue)
        {
            query = query.Where(i => i.CustomerId == customerId.Value);
        }

        var invoices = await query
            .Include(i => i.Customer)
            .Include(i => i.Items)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new
            {
                InvoiceId = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer.FullName,
                InvoiceDate = i.CreatedAt,
                Subtotal = i.TotalAmount,
                Discount = i.TotalAmount - i.FinalAmount,
                Total = i.FinalAmount,
                PaymentStatus = i.PaymentStatus,
                IsPaid = i.IsPaid,
                ItemCount = i.Items.Count
            })
            .Cast<dynamic>()
            .ToListAsync();

        return invoices;
    }

main
}
