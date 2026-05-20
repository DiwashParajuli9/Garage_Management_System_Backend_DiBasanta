using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Staff;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class StaffProfileService : IStaffProfileService
{
    private readonly AppDbContext _context;

    public StaffProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StaffProfileDto> GetProfileByIdAsync(Guid staffId)
    {
        var staff = await _context.Users
            .Where(u => u.Id == staffId && u.Role == UserRole.Staff)
            .FirstOrDefaultAsync();

        if (staff is null)
            throw new Exception("Staff member not found");

        return MapToDto(staff);
    }

    public async Task<StaffProfileDto> GetMyProfileAsync(Guid staffId)
    {
        return await GetProfileByIdAsync(staffId);
    }

    public async Task<List<StaffSalesHistoryDto>> GetMyProfileSalesHistoryAsync(Guid staffId)
    {
        var invoices = await _context.SalesInvoices
            .Where(i => i.StaffId == staffId)
            .OrderByDescending(i => i.CreatedAt)
            .Take(20)
            .Select(i => new StaffSalesHistoryDto
            {
                InvoiceId = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerName = i.Customer.FullName,
                InvoiceDate = i.CreatedAt,
                Amount = i.GrandTotal,
                PaymentStatus = i.PaymentStatus,
                ItemCount = i.Items.Count
            })
            .ToListAsync();

        return invoices;
    }

    public async Task<StaffProfileDto> UpdateProfileAsync(Guid staffId, UpdateStaffProfileDto dto)
    {
        var staff = await _context.Users
            .Where(u => u.Id == staffId && u.Role == UserRole.Staff)
            .FirstOrDefaultAsync();

        if (staff is null)
            throw new Exception("Staff member not found");

        staff.FullName = dto.FullName;
        staff.Phone = dto.Phone;

        _context.Users.Update(staff);
        await _context.SaveChangesAsync();

        return MapToDto(staff);
    }

    private StaffProfileDto MapToDto(Models.Entities.User staff)
    {
        return new StaffProfileDto
        {
            Id = staff.Id,
            FullName = staff.FullName,
            Email = staff.Email,
            Phone = staff.Phone,
            JobTitle = "Staff", // TODO: Add JobTitle field to Staff entity if needed
            Department = "Garage", // TODO: Add Department field to Staff entity if needed
            Role = staff.Role.ToString(),
            Status = "Active",
            CreatedAt = staff.CreatedAt,
            LastLoginAt = DateTime.UtcNow,
            Permissions = new List<string> { "read_invoices", "create_invoices", "manage_appointments", "manage_parts" }
        };
    }
}
