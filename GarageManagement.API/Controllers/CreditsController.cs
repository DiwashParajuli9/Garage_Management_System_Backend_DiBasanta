using GarageManagement.API.Models.DTOs.Sales;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/credits")]
[Authorize(Roles = "Staff,Admin")]
public class CreditsController : ControllerBase
{
    private readonly ICreditService _creditService;

    public CreditsController(ICreditService creditService)
    {
        _creditService = creditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCredits()
    {
        try
        {
            var credits = await _creditService.GetAllCreditsAsync();
            return Ok(credits);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetCreditSummary()
    {
        try
        {
            var summary = await _creditService.GetCreditSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{invoiceId:guid}")]
    public async Task<IActionResult> GetCreditByInvoiceId(Guid invoiceId)
    {
        try
        {
            var credit = await _creditService.GetCreditByInvoiceIdAsync(invoiceId);
            return Ok(credit);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("payment")]
    public async Task<IActionResult> RecordCreditPayment([FromBody] CreditPaymentDto dto)
    {
        try
        {
            var credit = await _creditService.RecordCreditPaymentAsync(dto);
            return Ok(credit);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{invoiceId:guid}/reminder")]
    public async Task<IActionResult> SendCreditReminder(Guid invoiceId, [FromBody] dynamic? request = null)
    {
        try
        {
            var customMessage = request?.message as string;
            await _creditService.SendCreditReminderAsync(invoiceId, customMessage);
            return Ok(new { message = "Reminder sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
