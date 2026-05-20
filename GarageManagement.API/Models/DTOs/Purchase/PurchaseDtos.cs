using System;
using System.Collections.Generic;

namespace GarageManagement.API.Models.DTOs.Purchase;

public class PurchaseItemDto
{
    public Guid VehiclePartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreatePurchaseInvoiceDto
{
    public Guid VendorId { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
}

public class PurchaseItemResponseDto
{
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class PurchaseInvoiceResponseDto
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public List<PurchaseItemResponseDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}
