using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.PartRequests;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class PartRequestService : IPartRequestService
{
    private readonly AppDbContext _context;

    public PartRequestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PartRequestDto> CreatePartRequestAsync(CreatePartRequestDto dto)
    {
        var customer = await _context.Users.FindAsync(dto.CustomerId);
        var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);

        if (customer is null)
            throw new Exception("Customer not found");
        if (vehicle is null)
            throw new Exception("Vehicle not found");

        var partRequest = new PartRequest
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            VehicleId = dto.VehicleId,
            PartName = dto.PartName,
            Status = "Pending",
            CustomerNotes = dto.CustomerNotes,
            StaffComment = string.Empty
        };

        _context.PartRequests.Add(partRequest);
        await _context.SaveChangesAsync();

        return MapToDto(partRequest, customer, vehicle);
    }

    public async Task<List<PartRequestDto>> GetPartRequestsAsync(string? status = null, string? searchTerm = null)
    {
        var query = _context.PartRequests
            .Include(pr => pr.Customer)
            .Include(pr => pr.Vehicle)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(pr => pr.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(pr =>
                pr.PartName.ToLower().Contains(search) ||
                pr.Customer.FullName.ToLower().Contains(search) ||
                pr.Vehicle.VehicleNumber.ToLower().Contains(search));
        }

        var requests = await query
            .OrderByDescending(pr => pr.CreatedAt)
            .Select(pr => MapToDto(pr, pr.Customer, pr.Vehicle))
            .ToListAsync();

        return requests;
    }

    public async Task<PartRequestDto> GetPartRequestByIdAsync(Guid partRequestId)
    {
        var request = await _context.PartRequests
            .Include(pr => pr.Customer)
            .Include(pr => pr.Vehicle)
            .FirstOrDefaultAsync(pr => pr.Id == partRequestId);

        if (request is null)
            throw new Exception("Part request not found");

        return MapToDto(request, request.Customer, request.Vehicle);
    }

    public async Task<PartRequestDto> UpdatePartRequestAsync(Guid partRequestId, UpdatePartRequestDto dto)
    {
        var request = await _context.PartRequests
            .Include(pr => pr.Customer)
            .Include(pr => pr.Vehicle)
            .FirstOrDefaultAsync(pr => pr.Id == partRequestId);

        if (request is null)
            throw new Exception("Part request not found");

        if (!string.IsNullOrEmpty(dto.Status))
            request.Status = dto.Status;
        if (!string.IsNullOrEmpty(dto.StaffComment))
            request.StaffComment = dto.StaffComment;

        _context.PartRequests.Update(request);
        await _context.SaveChangesAsync();

        return MapToDto(request, request.Customer, request.Vehicle);
    }

    public async Task<List<PartRequestCommentHistoryDto>> GetPartRequestCommentHistoryAsync(Guid partRequestId)
    {
        var request = await _context.PartRequests.FindAsync(partRequestId);
        if (request is null)
            throw new Exception("Part request not found");

        // TODO: Implement comment history tracking - for now return empty list
        return new List<PartRequestCommentHistoryDto>
        {
            new()
            {
                Id = request.Id,
                UpdatedAt = request.UpdatedAt,
                Status = request.Status,
                Comment = request.StaffComment,
                UpdatedBy = "System"
            }
        };
    }

    public async Task DeletePartRequestAsync(Guid partRequestId)
    {
        var request = await _context.PartRequests.FindAsync(partRequestId);
        if (request is null)
            throw new Exception("Part request not found");

        _context.PartRequests.Remove(request);
        await _context.SaveChangesAsync();
    }

    private PartRequestDto MapToDto(PartRequest request, Models.Entities.User customer, Vehicle vehicle)
    {
        return new PartRequestDto
        {
            Id = request.Id,
            CustomerId = request.CustomerId,
            CustomerName = customer.FullName,
            PartName = request.PartName,
            VehicleId = request.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            RequestDate = request.CreatedAt,
            Status = request.Status,
            CustomerNotes = request.CustomerNotes,
            StaffComment = request.StaffComment
        };
    }
}
