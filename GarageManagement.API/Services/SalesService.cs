using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Sales;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using GarageManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;

    public SalesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesInvoiceResponseDto> CreateSalesInvoice(Guid staffId, CreateSalesInvoiceDto dto)
    {
        var customer = await _context.Users.FirstOrDefaultAsync(user => user.Id == dto.CustomerId);
        if (customer is null)
        {
            throw new Exception("Customer not found");
        }

        var staff = await _context.Users.FirstOrDefaultAsync(user => user.Id == staffId);
        if (staff is null)
        {
            throw new Exception("Staff not found");
        }

        if (dto.Items.Count == 0)
        {
            throw new Exception("At least one item is required");
        }

        decimal totalAmount = 0;
        var invoiceItems = new List<SalesInvoiceItem>();
        var touchedParts = new List<VehiclePart>();

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new Exception("Item quantity must be greater than zero");
            }

            var part = await _context.VehicleParts.FirstOrDefaultAsync(vehiclePart => vehiclePart.Id == item.VehiclePartId);
            if (part is null)
            {
                throw new Exception($"Vehicle part with id {item.VehiclePartId} not found");
            }

            if (part.StockQuantity < item.Quantity)
            {
                throw new Exception($"Insufficient stock for part {part.Name}. Available: {part.StockQuantity}, Requested: {item.Quantity}");
            }

            part.StockQuantity -= item.Quantity;
            totalAmount += item.Quantity * part.Price;

            invoiceItems.Add(new SalesInvoiceItem
            {
                Id = Guid.NewGuid(),
                VehiclePartId = part.Id,
                Quantity = item.Quantity,
                UnitPrice = part.Price
            });

            touchedParts.Add(part);
        }

        var discountApplied = totalAmount > 5000m;
        var discountAmount = discountApplied ? totalAmount * 0.10m : 0m;
        var finalAmount = totalAmount - discountAmount;

        var invoice = new SalesInvoice
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            StaffId = staffId,
            TotalAmount = totalAmount,
            DiscountApplied = discountApplied,
            FinalAmount = finalAmount,
            IsPaid = false,
            Items = invoiceItems
        };

        _context.SalesInvoices.Add(invoice);
        await _context.SaveChangesAsync();

        var partsBelowThreshold = touchedParts
            .Where(part => part.StockQuantity < 10)
            .DistinctBy(part => part.Id)
            .ToList();

        if (partsBelowThreshold.Count > 0)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(user => user.Role == UserRole.Admin);
            if (admin is not null)
            {
                foreach (var lowStockPart in partsBelowThreshold)
                {
                    _context.Notifications.Add(new Notification
                    {
                        Id = Guid.NewGuid(),
                        UserId = admin.Id,
                        Message = $"Low stock alert: {lowStockPart.Name} has {lowStockPart.StockQuantity} items remaining.",
                        IsRead = false
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        var savedInvoice = await _context.SalesInvoices
            .Include(salesInvoice => salesInvoice.Customer)
            .Include(salesInvoice => salesInvoice.Staff)
            .Include(salesInvoice => salesInvoice.Items)
            .ThenInclude(item => item.VehiclePart)
            .FirstAsync(salesInvoice => salesInvoice.Id == invoice.Id);

        return MapInvoice(savedInvoice);
    }

    public async Task<List<SalesInvoiceResponseDto>> GetAllInvoices()
    {
        var invoices = await _context.SalesInvoices
            .Include(salesInvoice => salesInvoice.Customer)
            .Include(salesInvoice => salesInvoice.Staff)
            .Include(salesInvoice => salesInvoice.Items)
            .ThenInclude(item => item.VehiclePart)
            .OrderByDescending(salesInvoice => salesInvoice.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapInvoice).ToList();
    }

    public async Task<SalesInvoiceResponseDto> GetInvoiceById(Guid id)
    {
        var invoice = await _context.SalesInvoices
            .Include(salesInvoice => salesInvoice.Customer)
            .Include(salesInvoice => salesInvoice.Staff)
            .Include(salesInvoice => salesInvoice.Items)
            .ThenInclude(item => item.VehiclePart)
            .FirstOrDefaultAsync(salesInvoice => salesInvoice.Id == id);

        if (invoice is null)
        {
            throw new Exception("Invoice not found");
        }

        return MapInvoice(invoice);
    }

    public async Task<SalesInvoiceResponseDto> MarkAsPaid(Guid invoiceId)
    {
        var invoice = await _context.SalesInvoices
            .Include(salesInvoice => salesInvoice.Customer)
            .Include(salesInvoice => salesInvoice.Staff)
            .Include(salesInvoice => salesInvoice.Items)
            .ThenInclude(item => item.VehiclePart)
            .FirstOrDefaultAsync(salesInvoice => salesInvoice.Id == invoiceId);

        if (invoice is null)
        {
            throw new Exception("Invoice not found");
        }

        invoice.IsPaid = true;
        await _context.SaveChangesAsync();

        return MapInvoice(invoice);
    }

    private static SalesInvoiceResponseDto MapInvoice(SalesInvoice invoice)
    {
        return new SalesInvoiceResponseDto
        {
            Id = invoice.Id,
            CustomerId = invoice.CustomerId,
            CustomerName = invoice.Customer.FullName,
            StaffName = invoice.Staff.FullName,
            Items = invoice.Items.Select(item => new SalesItemResponseDto
            {
                PartName = item.VehiclePart.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Subtotal = item.UnitPrice * item.Quantity
            }).ToList(),
            TotalAmount = invoice.TotalAmount,
            DiscountApplied = invoice.DiscountApplied,
            DiscountAmount = invoice.TotalAmount - invoice.FinalAmount,
            FinalAmount = invoice.FinalAmount,
            IsPaid = invoice.IsPaid,
            CreatedAt = invoice.CreatedAt
        };
    }
}