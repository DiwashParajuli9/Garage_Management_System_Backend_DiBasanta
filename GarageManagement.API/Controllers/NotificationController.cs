using GarageManagement.API.Data;
using GarageManagement.API.Models.DTOs.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotifications()
    {
        try
        {
            var notifications = await _context.Notifications
                .OrderByDescending(notification => notification.CreatedAt)
                .Select(notification => new NotificationDto
                {
                    Id = notification.Id,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt
                })
                .ToListAsync();

            return Ok(notifications);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        try
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(item => item.Id == id);
            if (notification is null)
            {
                throw new Exception("Notification not found");
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok(notification.Id);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        try
        {
            var unreadNotifications = await _context.Notifications
                .Where(notification => !notification.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}