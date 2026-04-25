using GarageManagement.API.Models.DTOs.Auth;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto dto)
    {
        try
        {
            var response = await _authService.RegisterCustomer(dto);
            return Created(string.Empty, response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await _authService.Login(dto);
            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register-staff")]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterStaffDto dto)
    {
        try
        {
            var response = await _authService.RegisterStaff(dto);
            return Created(string.Empty, response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("staff")]
    public async Task<IActionResult> GetAllStaff()
    {
        try
        {
            var staff = await _authService.GetAllStaff();
            return Ok(staff);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("staff/{id:guid}")]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        try
        {
            await _authService.DeleteStaff(id);
            return NoContent();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}