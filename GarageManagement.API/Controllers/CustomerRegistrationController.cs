using GarageManagement.API.Models.DTOs.CustomerRegistration;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

/// <summary>
/// F6 – Customer Registration
/// Public endpoint: registers a new customer + optional vehicle in one request.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerRegistrationController : ControllerBase
{
    private readonly ICustomerRegistrationService _registrationService;

    public CustomerRegistrationController(ICustomerRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    // ── POST api/customerregistration ───────────────────────────────────────
    /// <summary>
    /// Register a new customer account, with an optional vehicle attached.
    /// No authentication required – this is the public sign-up endpoint.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto dto)
    {
        try
        {
            var result = await _registrationService.RegisterCustomerAsync(dto);
            return Created($"api/customer/profile", result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
