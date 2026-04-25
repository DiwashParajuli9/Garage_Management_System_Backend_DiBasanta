using GarageManagement.API.Models.DTOs.Sales;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesInvoice([FromBody] CreateSalesInvoiceDto dto)
    {
        try
        {
            var userIdStr = HttpContext.Items["UserId"]?.ToString();
            var staffId = Guid.Parse(userIdStr!);
            var invoice = await _salesService.CreateSalesInvoice(staffId, dto);
            return Created(string.Empty, invoice);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllInvoices()
    {
        try
        {
            var invoices = await _salesService.GetAllInvoices();
            return Ok(invoices);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        try
        {
            var invoice = await _salesService.GetInvoiceById(id);
            return Ok(invoice);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize(Roles = "Staff,Admin")]
    [HttpPut("{id:guid}/pay")]
    public async Task<IActionResult> MarkAsPaid(Guid id)
    {
        try
        {
            var invoice = await _salesService.MarkAsPaid(id);
            return Ok(invoice);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}