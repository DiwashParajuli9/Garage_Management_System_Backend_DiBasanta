using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

/// <summary>
/// F9 – Customer Reports
/// Admin-only analytics endpoints for customer data.
/// </summary>
[ApiController]
[Route("api/reports/customers")]
[Authorize(Roles = "Admin")]
public class CustomerReportController : ControllerBase
{
    private readonly ICustomerReportService _reportService;

    public CustomerReportController(ICustomerReportService reportService)
    {
        _reportService = reportService;
    }

    // ── GET api/reports/customers/total ─────────────────────────────────────
    /// <summary>Returns the total number of registered customers.</summary>
    [HttpGet("total")]
    public async Task<IActionResult> GetTotalCustomers()
    {
        try
        {
            var result = await _reportService.GetTotalCustomersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── GET api/reports/customers/by-vehicle ────────────────────────────────
    /// <summary>
    /// Returns customer counts grouped by vehicle make (most common first).
    /// </summary>
    [HttpGet("by-vehicle")]
    public async Task<IActionResult> GetCustomersByVehicle()
    {
        try
        {
            var result = await _reportService.GetCustomersByVehicleAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── GET api/reports/customers/recent?count=10 ───────────────────────────
    /// <summary>
    /// Returns the N most recently registered customers.
    /// Query param <c>count</c> defaults to 10; max 100.
    /// </summary>
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentCustomers([FromQuery] int count = 10)
    {
        if (count < 1 || count > 100)
            return BadRequest(new { message = "count must be between 1 and 100." });

        try
        {
            var result = await _reportService.GetRecentCustomersAsync(count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
feature/diwash-F1-F7-F12-F15

    // ── GET api/reports/customers/service-history ──────────────────────────
    /// <summary>Returns customer service history (sales invoices).</summary>
    [HttpGet("service-history")]
    public async Task<IActionResult> GetCustomerServiceHistory([FromQuery] Guid? customerId = null)
    {
        try
        {
            var result = await _reportService.GetCustomerServiceHistoryAsync(customerId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // ── GET api/reports/customers/sales-invoices ───────────────────────────
    /// <summary>Returns customer sales invoices.</summary>
    [HttpGet("sales-invoices")]
    public async Task<IActionResult> GetCustomerSalesInvoices([FromQuery] Guid? customerId = null)
    {
        try
        {
            var result = await _reportService.GetCustomerSalesInvoicesAsync(customerId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
main
