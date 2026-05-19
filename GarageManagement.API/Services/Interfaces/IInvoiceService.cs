using System;
using System.Threading.Tasks;

namespace GarageManagement.API.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task SendInvoiceEmailAsync(Guid invoiceId);
    }
}
