using GarageManagement.API.Data;
using GarageManagement.API.Helpers;
using GarageManagement.API.Models.DTOs.Sales;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class CreditService : ICreditService
{
    private readonly AppDbContext _context;
    private readonly EmailHelper _emailHelper;

    public CreditService(AppDbContext context, EmailHelper emailHelper)
    {
        _context = context;
        _emailHelper = emailHelper;
    }

    public async Task<List<CreditDto>> GetAllCreditsAsync()
    {
        var credits = await _context.SalesInvoices
            .Where(i => i.PaymentStatus == "Credit")
            .Include(i => i.Customer)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => MapToDto(i))
            .ToListAsync();

        return credits;
    }

    public async Task<CreditDto> GetCreditByInvoiceIdAsync(Guid invoiceId)
    {
        var invoice = await _context.SalesInvoices
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == invoiceId && i.PaymentStatus == "Credit");

        if (invoice is null)
            throw new Exception("Credit invoice not found");

        return MapToDto(invoice);
    }

    public async Task<CreditSummaryDto> GetCreditSummaryAsync()
    {
        var credits = await _context.SalesInvoices
            .Where(i => i.PaymentStatus == "Credit")
            .ToListAsync();

        var totalOutstanding = credits.Sum(c => c.GrandTotal);
        var overdueCredits = credits.Where(c => c.CreatedAt.AddDays(30) < DateTime.UtcNow).ToList();
        var totalOverdue = overdueCredits.Sum(c => c.GrandTotal);

        return new CreditSummaryDto
        {
            ActiveCreditCount = credits.Count,
            OverdueCount = overdueCredits.Count,
            TotalOutstanding = totalOutstanding,
            TotalOverdue = totalOverdue
        };
    }

    public async Task<CreditDto> RecordCreditPaymentAsync(CreditPaymentDto dto)
    {
        var invoice = await _context.SalesInvoices
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId);

        if (invoice is null)
            throw new Exception("Invoice not found");

        // Update invoice payment status
        var remainingAmount = invoice.GrandTotal - dto.PaidAmount;
        if (remainingAmount <= 0)
        {
            invoice.PaymentStatus = "Paid";
        }

        _context.SalesInvoices.Update(invoice);
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task SendCreditReminderAsync(Guid invoiceId, string? customMessage = null)
    {
        var invoice = await _context.SalesInvoices
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(i => i.Id == invoiceId && i.PaymentStatus == "Credit");

        if (invoice is null)
            throw new Exception("Credit invoice not found");

        var daysOverdue = (int)(DateTime.UtcNow - invoice.CreatedAt.AddDays(30)).TotalDays;
        var subject = daysOverdue > 0 ? $"Payment Reminder - Overdue Invoice {invoice.InvoiceNumber}" 
                                      : $"Upcoming Payment Due - Invoice {invoice.InvoiceNumber}";

        var body = customMessage ?? $@"
Dear {invoice.Customer.FullName},

This is a reminder that your invoice {invoice.InvoiceNumber} is due.

Invoice Date: {invoice.CreatedAt:yyyy-MM-dd}
Amount Due: {invoice.GrandTotal:C}
Due Date: {invoice.CreatedAt.AddDays(30):yyyy-MM-dd}

{(daysOverdue > 0 ? $"This invoice is now {daysOverdue} days overdue. Please settle at your earliest convenience." : "Please arrange payment by the due date.")}

Thank you.
";

        try
        {
            await _emailHelper.SendEmailAsync(invoice.Customer.Email, subject, body);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to send reminder email: {ex.Message}");
        }
    }

    private CreditDto MapToDto(Models.Entities.SalesInvoice invoice)
    {
        var isOverdue = invoice.CreatedAt.AddDays(30) < DateTime.UtcNow;
        var overdueDays = isOverdue ? (int)(DateTime.UtcNow - invoice.CreatedAt.AddDays(30)).TotalDays : 0;

        return new CreditDto
        {
            InvoiceId = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            CustomerId = invoice.CustomerId,
            CustomerName = invoice.Customer.FullName,
            GrandTotal = invoice.GrandTotal,
            PaidAmount = invoice.PaymentStatus == "Paid" ? invoice.GrandTotal : 0,
            RemainingAmount = invoice.PaymentStatus == "Paid" ? 0 : invoice.GrandTotal,
            PaymentStatus = invoice.PaymentStatus,
            PurchaseDate = invoice.CreatedAt,
            DueDate = invoice.CreatedAt.AddDays(30),
            IsOverdue = isOverdue,
            OverdueDays = overdueDays
        };
    }
}
