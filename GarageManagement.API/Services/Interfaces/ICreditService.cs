using GarageManagement.API.Models.DTOs.Sales;

namespace GarageManagement.API.Services.Interfaces;

public interface ICreditService
{
    Task<List<CreditDto>> GetAllCreditsAsync();
    Task<CreditDto> GetCreditByInvoiceIdAsync(Guid invoiceId);
    Task<CreditSummaryDto> GetCreditSummaryAsync();
    Task<CreditDto> RecordCreditPaymentAsync(CreditPaymentDto dto);
    Task SendCreditReminderAsync(Guid invoiceId, string? customMessage = null);
}
