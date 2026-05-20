using GarageManagement.API.Services.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GarageManagement.API.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Mock implementation: Log to debug output
            Debug.WriteLine($"Sending email to: {to}");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Body: {body}");

            // Simulate async work
            await Task.Delay(100);
        }
    }
}
