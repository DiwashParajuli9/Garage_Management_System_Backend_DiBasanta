using GarageManagement.API.Models.DTOs.Sales;

namespace GarageManagement.API.Services.Interfaces;

public interface ISalesService
{
    Task<SalesInvoiceResponseDto> CreateSalesInvoice(Guid staffId, CreateSalesInvoiceDto dto);
    Task<List<SalesInvoiceResponseDto>> GetAllInvoices();
    Task<SalesInvoiceResponseDto> GetInvoiceById(Guid id);
    Task<SalesInvoiceResponseDto> MarkAsPaid(Guid invoiceId);
    Task<bool> SendInvoiceEmailAsync(Guid invoiceId, string recipientEmail, string subject = "Your Invoice", string? message = null);
}