using GarageManagement.API.Data;
using GarageManagement.API.Models.Entities;
using GarageManagement.API.Models.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Services.Background;

public class LowStockNotificationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LowStockNotificationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var lowStockParts = await context.VehicleParts
                .Where(part => part.StockQuantity < 10)
                .ToListAsync(stoppingToken);

            if (lowStockParts.Count > 0)
            {
                var admin = await context.Users
                    .FirstOrDefaultAsync(user => user.Role == UserRole.Admin, stoppingToken);

                if (admin is not null)
                {
                    var today = DateTime.UtcNow.Date;
                    var todayNotifications = await context.Notifications
                        .Where(notification => notification.CreatedAt.Date == today)
                        .ToListAsync(stoppingToken);

                    var newNotifications = new List<Notification>();

                    foreach (var part in lowStockParts)
                    {
                        var alreadyNotifiedToday = todayNotifications.Any(notification =>
                            notification.Message.Contains(part.Name, StringComparison.OrdinalIgnoreCase));

                        if (!alreadyNotifiedToday)
                        {
                            newNotifications.Add(new Notification
                            {
                                Id = Guid.NewGuid(),
                                UserId = admin.Id,
                                Message = $"Low stock alert: {part.Name} has only {part.StockQuantity} units remaining",
                                IsRead = false
                            });
                        }
                    }

                    if (newNotifications.Count > 0)
                    {
                        context.Notifications.AddRange(newNotifications);
                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}