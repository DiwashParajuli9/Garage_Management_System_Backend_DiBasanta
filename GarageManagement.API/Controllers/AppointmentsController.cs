using GarageManagement.API.Models.DTOs.Appointments;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize(Roles = "Staff,Admin,Customer")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.CreateAppointmentAsync(dto);
            return Created($"api/appointments/{appointment.Id}", appointment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery] AppointmentFilterDto filter)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(filter);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetAppointmentStats()
    {
        try
        {
            var stats = await _appointmentService.GetAppointmentStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{appointmentId:guid}")]
    public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{appointmentId:guid}")]
    public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAppointmentAsync(appointmentId, dto);
            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{appointmentId:guid}")]
    public async Task<IActionResult> DeleteAppointment(Guid appointmentId)
    {
        try
        {
            await _appointmentService.DeleteAppointmentAsync(appointmentId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
