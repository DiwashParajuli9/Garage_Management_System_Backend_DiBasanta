using GarageManagement.API.Models.DTOs.Purchase;
using GarageManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PurchaseInvoiceController : ControllerBase
{
    private readonly IPurchaseInvoiceService _service;

    public PurchaseInvoiceController(IPurchaseInvoiceService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInvoice([FromBody] CreatePurchaseInvoiceDto dto)
    {
        try
        {
            var userIdStr = HttpContext.Items["UserId"]?.ToString();
            var adminId = Guid.Parse(userIdStr!);
            var invoice = await _service.CreatePurchaseInvoice(adminId, dto);
            return Created(string.Empty, invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var invoices = await _service.GetAllInvoices();
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var invoice = await _service.GetInvoiceById(id);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
