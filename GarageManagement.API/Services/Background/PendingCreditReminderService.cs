using GarageManagement.API.Data;
using GarageManagement.API.Helpers;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services.Background;

public class PendingCreditReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PendingCreditReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailHelper = scope.ServiceProvider.GetRequiredService<EmailHelper>();

            var overdueInvoices = await context.SalesInvoices
                .Include(invoice => invoice.Customer)
                .Where(invoice => !invoice.IsPaid && invoice.CreatedAt < DateTime.UtcNow.AddDays(-30))
                .ToListAsync(stoppingToken);

            if (overdueInvoices.Count > 0)
            {
                var admin = await context.Users
                    .FirstOrDefaultAsync(user => user.Role == UserRole.Admin, stoppingToken);

                var notifications = new List<Notification>();

                foreach (var invoice in overdueInvoices)
                {
                    var customer = invoice.Customer;
                    var subject = $"Payment Reminder - Invoice #{invoice.Id}";
                    var htmlBody = $@"
<html>
  <body>
    <p>Dear {customer.FullName},</p>
    <p>This is a reminder that your invoice is overdue.</p>
    <p><strong>Invoice ID:</strong> {invoice.Id}</p>
    <p><strong>Invoice Amount:</strong> {invoice.FinalAmount}</p>
    <p><strong>Date Created:</strong> {invoice.CreatedAt:yyyy-MM-dd}</p>
    <p>Please complete the payment at your earliest convenience.</p>
    <p>Thank you,<br/>Garage Management Team</p>
  </body>
</html>";

                    await emailHelper.SendEmailAsync(customer.Email, customer.FullName, subject, htmlBody);

                    if (admin is not null)
                    {
                        notifications.Add(new Notification
                        {
                            Id = Guid.NewGuid(),
                            UserId = admin.Id,
                            Message = $"Credit reminder sent to {customer.FullName} for invoice #{invoice.Id} - Amount: {invoice.FinalAmount}",
                            IsRead = false
                        });
                    }
                }

                if (notifications.Count > 0)
                {
                    context.Notifications.AddRange(notifications);
                    await context.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}