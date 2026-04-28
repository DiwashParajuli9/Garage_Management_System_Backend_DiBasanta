using GarageManagement.API.Models.DTOs.Staff;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

/// <summary>
/// F2 – Staff Management
/// CRUD endpoints for managing garage staff members.
/// Restricted to Admin role only.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    // ── POST api/staff ──────────────────────────────────────────────────────
    /// <summary>Create a new staff member.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto dto)
    {
        try
        {
            var staff = await _staffService.CreateStaffAsync(dto);
            return Created($"api/staff/{staff.Id}", staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── GET api/staff ───────────────────────────────────────────────────────
    /// <summary>Retrieve all staff members.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── GET api/staff/{id} ──────────────────────────────────────────────────
    /// <summary>Retrieve a single staff member by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStaffById(Guid id)
    {
        try
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // ── PUT api/staff/{id} ──────────────────────────────────────────────────
    /// <summary>Update an existing staff member.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffDto dto)
    {
        try
        {
            var staff = await _staffService.UpdateStaffAsync(id, dto);
            return Ok(staff);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── DELETE api/staff/{id} ───────────────────────────────────────────────
    /// <summary>Delete a staff member.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        try
        {
            await _staffService.DeleteStaffAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
