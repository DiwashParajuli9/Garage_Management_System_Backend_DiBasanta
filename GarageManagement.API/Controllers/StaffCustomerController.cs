using GarageManagement.API.Models.DTOs.Customer;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/staff/customers")]
[Authorize(Roles = "Staff,Admin")]
public class StaffCustomerController : ControllerBase
{
    private readonly IStaffCustomerService _customerService;

    public StaffCustomerController(IStaffCustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid customerId)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string? q = null)
    {
        try
        {
            var results = await _customerService.SearchCustomersAsync(q ?? "");
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{customerId:guid}/service-history")]
    public async Task<IActionResult> GetCustomerServiceHistory(Guid customerId)
    {
        try
        {
            var history = await _customerService.GetCustomerServiceHistoryAsync(customerId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateStaffCustomerDto dto)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(dto);
            return Created($"api/staff/customers/{customer.Id}", customer);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{customerId:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateStaffCustomerDto dto)
    {
        try
        {
            var customer = await _customerService.UpdateCustomerAsync(customerId, dto);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{customerId:guid}")]
    public async Task<IActionResult> DeleteCustomer(Guid customerId)
    {
        try
        {
            await _customerService.DeleteCustomerAsync(customerId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
