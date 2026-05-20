using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Purchase;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class PurchaseInvoiceService : IPurchaseInvoiceService
{
    private readonly AppDbContext _context;

    public PurchaseInvoiceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseInvoiceResponseDto> CreatePurchaseInvoice(Guid adminId, CreatePurchaseInvoiceDto dto)
    {
        var vendor = await _context.Vendors.FirstOrDefaultAsync(v => v.Id == dto.VendorId);
        if (vendor is null) throw new Exception("Vendor not found");

        if (dto.Items.Count == 0) throw new Exception("At least one item is required");

        decimal totalAmount = 0m;
        var invoiceItems = new List<PurchaseInvoiceItem>();
        var touchedParts = new List<VehiclePart>();

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0) throw new Exception("Item quantity must be greater than zero");

            var part = await _context.VehicleParts.FirstOrDefaultAsync(p => p.Id == item.VehiclePartId);
            if (part is null) throw new Exception($"Vehicle part with id {item.VehiclePartId} not found");

            part.StockQuantity += item.Quantity;
            totalAmount += item.Quantity * item.UnitPrice;

            invoiceItems.Add(new PurchaseInvoiceItem
            {
                Id = Guid.NewGuid(),
                VehiclePartId = part.Id,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });

            touchedParts.Add(part);
        }

        var invoice = new PurchaseInvoice
        {
            Id = Guid.NewGuid(),
            VendorId = dto.VendorId,
            AdminId = adminId,
            TotalAmount = totalAmount,
            Items = invoiceItems
        };

        _context.PurchaseInvoices.Add(invoice);
        await _context.SaveChangesAsync();

        var savedInvoice = await _context.PurchaseInvoices
            .Include(pi => pi.Vendor)
            .Include(pi => pi.Admin)
            .Include(pi => pi.Items)
                .ThenInclude(i => i.VehiclePart)
            .FirstAsync(pi => pi.Id == invoice.Id);

        return MapInvoice(savedInvoice);
    }

    public async Task<List<PurchaseInvoiceResponseDto>> GetAllInvoices()
    {
        var invoices = await _context.PurchaseInvoices
            .Include(pi => pi.Vendor)
            .Include(pi => pi.Admin)
            .Include(pi => pi.Items)
                .ThenInclude(i => i.VehiclePart)
            .OrderByDescending(pi => pi.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapInvoice).ToList();
    }

    public async Task<PurchaseInvoiceResponseDto> GetInvoiceById(Guid id)
    {
        var invoice = await _context.PurchaseInvoices
            .Include(pi => pi.Vendor)
            .Include(pi => pi.Admin)
            .Include(pi => pi.Items)
                .ThenInclude(i => i.VehiclePart)
            .FirstOrDefaultAsync(pi => pi.Id == id);

        if (invoice is null) throw new Exception("Invoice not found");

        return MapInvoice(invoice);
    }

    private static PurchaseInvoiceResponseDto MapInvoice(PurchaseInvoice invoice)
    {
        return new PurchaseInvoiceResponseDto
        {
            Id = invoice.Id,
            VendorId = invoice.VendorId,
            VendorName = invoice.Vendor.Name,
            AdminName = invoice.Admin.FullName,
            Items = invoice.Items.Select(i => new PurchaseItemResponseDto
            {
                PartName = i.VehiclePart.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Quantity * i.UnitPrice
            }).ToList(),
            TotalAmount = invoice.TotalAmount,
            CreatedAt = invoice.CreatedAt
        };
    }
}
