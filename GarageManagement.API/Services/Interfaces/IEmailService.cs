using System.Threading.Tasks;

namespace GarageManagement.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
