using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Appointments;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var customer = await _context.Users.FindAsync(dto.CustomerId);
        var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);

        if (customer is null)
            throw new Exception("Customer not found");
        if (vehicle is null)
            throw new Exception("Vehicle not found");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            VehicleId = dto.VehicleId,
            AppointmentDate = dto.AppointmentDate,
            AppointmentTime = dto.AppointmentTime,
            ServiceType = dto.ServiceType,
            Status = "Pending",
            Notes = dto.Notes
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return MapToDto(appointment, customer, vehicle);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsAsync(AppointmentFilterDto filter)
    {
        var query = _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var search = filter.SearchTerm.ToLower();
            query = query.Where(a =>
                a.Customer.FullName.ToLower().Contains(search) ||
                a.Customer.Phone.Contains(search) ||
                a.Customer.Email.ToLower().Contains(search) ||
                a.Vehicle.VehicleNumber.ToLower().Contains(search));
        }

        // Apply status filter
        if (filter.Statuses?.Any() == true)
        {
            query = query.Where(a => filter.Statuses.Contains(a.Status));
        }

        // Apply date range filter
        if (filter.FromDate.HasValue)
            query = query.Where(a => a.AppointmentDate >= filter.FromDate);
        if (filter.ToDate.HasValue)
            query = query.Where(a => a.AppointmentDate <= filter.ToDate);

        var appointments = await query
            .OrderByDescending(a => a.AppointmentDate)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(a => MapToDto(a, a.Customer, a.Vehicle))
            .ToListAsync();

        return appointments;
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(Guid appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment is null)
            throw new Exception("Appointment not found");

        return MapToDto(appointment, appointment.Customer, appointment.Vehicle);
    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentDto dto)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Vehicle)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment is null)
            throw new Exception("Appointment not found");

        if (!string.IsNullOrEmpty(dto.Status))
            appointment.Status = dto.Status;
        if (dto.AppointmentDate.HasValue)
            appointment.AppointmentDate = dto.AppointmentDate.Value;
        if (dto.AppointmentTime.HasValue)
            appointment.AppointmentTime = dto.AppointmentTime.Value;
        if (!string.IsNullOrEmpty(dto.Notes))
            appointment.Notes = dto.Notes;

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();

        return MapToDto(appointment, appointment.Customer, appointment.Vehicle);
    }

    public async Task<AppointmentStatsDto> GetAppointmentStatsAsync()
    {
        var stats = new AppointmentStatsDto
        {
            PendingCount = await _context.Appointments.CountAsync(a => a.Status == "Pending"),
            ConfirmedCount = await _context.Appointments.CountAsync(a => a.Status == "Confirmed"),
            CompletedCount = await _context.Appointments.CountAsync(a => a.Status == "Completed"),
            RejectedCount = await _context.Appointments.CountAsync(a => a.Status == "Rejected")
        };
        return stats;
    }

    public async Task DeleteAppointmentAsync(Guid appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment is null)
            throw new Exception("Appointment not found");

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    private AppointmentDto MapToDto(Appointment appointment, Models.Entities.User customer, Vehicle vehicle)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            CustomerId = appointment.CustomerId,
            CustomerName = customer.FullName,
            CustomerPhone = customer.Phone,
            VehicleNumber = vehicle.VehicleNumber,
            AppointmentDate = appointment.AppointmentDate,
            AppointmentTime = appointment.AppointmentTime,
            ServiceType = appointment.ServiceType,
            Status = appointment.Status,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }
}
