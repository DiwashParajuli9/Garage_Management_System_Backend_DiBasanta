using GarageManagement.API.Models.DTOs.Staff;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/staff/profile")]
[Authorize(Roles = "Staff,Admin")]
public class StaffProfileController : ControllerBase
{
    private readonly IStaffProfileService _staffProfileService;

    public StaffProfileController(IStaffProfileService staffProfileService)
    {
        _staffProfileService = staffProfileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var staffId = GetStaffId();
            var profile = await _staffProfileService.GetMyProfileAsync(staffId);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("sales-history")]
    public async Task<IActionResult> GetMySalesHistory()
    {
        try
        {
            var staffId = GetStaffId();
            var history = await _staffProfileService.GetMyProfileSalesHistoryAsync(staffId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateStaffProfileDto dto)
    {
        try
        {
            var staffId = GetStaffId();
            var profile = await _staffProfileService.UpdateProfileAsync(staffId, dto);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetStaffId()
    {
        var userIdStr = HttpContext.Items["UserId"]?.ToString();
        return string.IsNullOrEmpty(userIdStr) ? Guid.Empty : Guid.Parse(userIdStr);
    }
}
