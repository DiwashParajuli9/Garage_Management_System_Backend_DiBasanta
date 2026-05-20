namespace GarageManagement.API.Models.DTOs.Sales;

public class CreditDto
{
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal GrandTotal { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public DateTime PurchaseDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsOverdue { get; set; }
    public int OverdueDays { get; set; }
}

public class CreditPaymentDto
{
    public Guid InvoiceId { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string Notes { get; set; } = string.Empty;
}

public class CreditSummaryDto
{
    public int ActiveCreditCount { get; set; }
    public int OverdueCount { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal TotalOverdue { get; set; }
}

public class SendInvoiceEmailDto
{
    public Guid InvoiceId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = "Your Invoice";
    public string Message { get; set; } = string.Empty;
}
