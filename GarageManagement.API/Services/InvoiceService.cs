using GarageManagement.API.Data;
using GarageManagement.API.Services.Interfaces;
using GarageManagement.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageManagement.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public InvoiceService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task SendInvoiceEmailAsync(Guid invoiceId)
        {
            var invoice = await _context.SalesInvoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                    .ThenInclude(item => item.VehiclePart)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
            {
                throw new Exception("Invoice not found");
            }

            var customerEmail = invoice.Customer.Email;
            var subject = $"[Garage Management] Invoice for your service - {invoice.Id}";
            
            var body = new StringBuilder();
            body.AppendLine("<html><body>");
            body.AppendLine($"<h2>Hello {invoice.Customer.FullName},</h2>");
            body.AppendLine("<p>Thank you for choosing our garage. Here are your invoice details:</p>");
            body.AppendLine("<table border='1' cellpadding='5' style='border-collapse: collapse;'>");
            body.AppendLine("<thead><tr><th>Part Name</th><th>Quantity</th><th>Unit Price</th><th>Total</th></tr></thead>");
            body.AppendLine("<tbody>");
            
            foreach (var item in invoice.Items)
            {
                var itemTotal = item.UnitPrice * item.Quantity;
                body.AppendLine("<tr>");
                body.AppendLine($"<td>{item.VehiclePart.Name}</td>");
                body.AppendLine($"<td>{item.Quantity}</td>");
                body.AppendLine($"<td>{item.UnitPrice:C}</td>");
                body.AppendLine($"<td>{itemTotal:C}</td>");
                body.AppendLine("</tr>");
            }
            
            body.AppendLine("</tbody></table>");
            body.AppendLine("<br/>");
            body.AppendLine($"<p><strong>Total Amount: {invoice.TotalAmount:C}</strong></p>");
            if (invoice.DiscountApplied)
            {
                body.AppendLine($"<p><strong>Final Amount (Discounted): {invoice.FinalAmount:C}</strong></p>");
            }
            body.AppendLine("<p>If you have any questions, please contact our support.</p>");
            body.AppendLine("<p>Best regards,<br/>Garage Management Team</p>");
            body.AppendLine("</body></html>");

            await _emailService.SendEmailAsync(customerEmail, subject, body.ToString());

            // Add notification
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = invoice.CustomerId,
                Message = $"An invoice for {invoice.FinalAmount:C} has been sent to your email ({customerEmail}).",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
