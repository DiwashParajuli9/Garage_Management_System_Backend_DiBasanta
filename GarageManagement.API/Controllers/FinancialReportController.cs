using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/reports/financial")]
[Authorize(Roles = "Admin")]
public class FinancialReportController : ControllerBase
{
    private readonly IFinancialReportService _financialReportService;

    public FinancialReportController(IFinancialReportService financialReportService)
    {
        _financialReportService = financialReportService;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
    {
        try
        {
            var report = await _financialReportService.GetDailyReport(date);
            return Ok(report);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
    {
        try
        {
            var report = await _financialReportService.GetMonthlyReport(year, month);
            return Ok(report);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("yearly")]
    public async Task<IActionResult> GetYearlyReport([FromQuery] int year)
    {
        try
        {
            var report = await _financialReportService.GetYearlyReport(year);
            return Ok(report);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}