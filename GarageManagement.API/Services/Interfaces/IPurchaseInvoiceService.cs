using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarageManagement.API.Models.DTOs.Purchase;

namespace GarageManagement.API.Services.Interfaces
{
    public interface IPurchaseInvoiceService
    {
        Task<PurchaseInvoiceResponseDto> CreatePurchaseInvoice(Guid adminId, CreatePurchaseInvoiceDto dto);
        Task<List<PurchaseInvoiceResponseDto>> GetAllInvoices();
        Task<PurchaseInvoiceResponseDto> GetInvoiceById(Guid id);
    }
}
