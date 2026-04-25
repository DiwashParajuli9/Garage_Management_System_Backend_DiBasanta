namespace GarageManagement.API.Models.DTOs.Sales;

public class SalesItemDto
{
    public Guid VehiclePartId { get; set; }
    public int Quantity { get; set; }
}

public class CreateSalesInvoiceDto
{
    public Guid CustomerId { get; set; }
    public List<SalesItemDto> Items { get; set; } = new();
}

public class SalesItemResponseDto
{
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class SalesInvoiceResponseDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public List<SalesItemResponseDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public bool DiscountApplied { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
}