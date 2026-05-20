using GarageManagement.API.Models.DTOs.Customer;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var customerId = GetCustomerId();
            var profile = await _customerService.GetMyProfile(customerId);
            return Ok(profile);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
    {
        try
        {
            var customerId = GetCustomerId();
            var profile = await _customerService.UpdateMyProfile(customerId, dto);
            return Ok(profile);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("vehicles")]
    public async Task<IActionResult> GetMyVehicles()
    {
        try
        {
            var customerId = GetCustomerId();
            var vehicles = await _customerService.GetMyVehicles(customerId);
            return Ok(vehicles);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("vehicles")]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleDto dto)
    {
        try
        {
            var customerId = GetCustomerId();
            var vehicle = await _customerService.AddVehicle(customerId, dto);
            return Created(string.Empty, vehicle);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("vehicles/{vehicleId:guid}")]
    public async Task<IActionResult> UpdateVehicle(Guid vehicleId, [FromBody] UpdateVehicleDto dto)
    {
        try
        {
            var customerId = GetCustomerId();
            var vehicle = await _customerService.UpdateVehicle(customerId, vehicleId, dto);
            return Ok(vehicle);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("vehicles/{vehicleId:guid}")]
    public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
    {
        try
        {
            var customerId = GetCustomerId();
            await _customerService.DeleteVehicle(customerId, vehicleId);
            return NoContent();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetMyHistory()
    {
        try
        {
            var customerId = GetCustomerId();
            var history = await _customerService.GetMyHistory(customerId);
            return Ok(history);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    private Guid GetCustomerId()
    {
        var userIdStr = HttpContext.Items["UserId"]?.ToString();
        var customerId = Guid.Parse(userIdStr!);
        return customerId;
    }
}