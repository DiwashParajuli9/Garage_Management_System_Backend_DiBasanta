using GarageManagement.API.Models.DTOs.Appointments;

namespace GarageManagement.API.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto);
    Task<List<AppointmentDto>> GetAppointmentsAsync(AppointmentFilterDto filter);
    Task<AppointmentDto> GetAppointmentByIdAsync(Guid appointmentId);
    Task<AppointmentDto> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentDto dto);
    Task<AppointmentStatsDto> GetAppointmentStatsAsync();
    Task DeleteAppointmentAsync(Guid appointmentId);
}
