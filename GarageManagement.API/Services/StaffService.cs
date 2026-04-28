using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Staff;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

/// <summary>
/// Implementation of IStaffService for F2 – Staff Management.
/// </summary>
public class StaffService : IStaffService
{
    private readonly AppDbContext _context;

    public StaffService(AppDbContext context)
    {
        _context = context;
    }

    // ── POST: Create ────────────────────────────────────────────────────────

    public async Task<StaffDto> CreateStaffAsync(CreateStaffDto dto)
    {
        // Guard: email must be unique
        var exists = await _context.Staff
            .AnyAsync(s => s.Email == dto.Email);

        if (exists)
            throw new Exception($"A staff member with email '{dto.Email}' already exists.");

        var staff = new Staff
        {
            Id        = Guid.NewGuid(),
            FullName  = dto.FullName,
            Email     = dto.Email,
            Phone     = dto.Phone,
            Position  = dto.Position,
            IsActive  = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        return MapToDto(staff);
    }

    // ── GET: All ────────────────────────────────────────────────────────────

    public async Task<List<StaffDto>> GetAllStaffAsync()
    {
        return await _context.Staff
            .OrderBy(s => s.FullName)
            .Select(s => MapToDto(s))
            .ToListAsync();
    }

    // ── GET: Single ─────────────────────────────────────────────────────────

    public async Task<StaffDto> GetStaffByIdAsync(Guid staffId)
    {
        var staff = await _context.Staff.FindAsync(staffId)
            ?? throw new Exception($"Staff member with ID '{staffId}' not found.");

        return MapToDto(staff);
    }

    // ── PUT: Update ─────────────────────────────────────────────────────────

    public async Task<StaffDto> UpdateStaffAsync(Guid staffId, UpdateStaffDto dto)
    {
        var staff = await _context.Staff.FindAsync(staffId)
            ?? throw new Exception($"Staff member with ID '{staffId}' not found.");

        staff.FullName  = dto.FullName;
        staff.Phone     = dto.Phone;
        staff.Position  = dto.Position;
        staff.IsActive  = dto.IsActive;
        staff.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDto(staff);
    }

    // ── DELETE ──────────────────────────────────────────────────────────────

    public async Task DeleteStaffAsync(Guid staffId)
    {
        var staff = await _context.Staff.FindAsync(staffId)
            ?? throw new Exception($"Staff member with ID '{staffId}' not found.");

        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync();
    }

    // ── Mapper ──────────────────────────────────────────────────────────────

    private static StaffDto MapToDto(Staff staff) => new StaffDto
    {
        Id        = staff.Id,
        FullName  = staff.FullName,
        Email     = staff.Email,
        Phone     = staff.Phone,
        Position  = staff.Position,
        IsActive  = staff.IsActive,
        CreatedAt = staff.CreatedAt,
        UpdatedAt = staff.UpdatedAt
    };
}
