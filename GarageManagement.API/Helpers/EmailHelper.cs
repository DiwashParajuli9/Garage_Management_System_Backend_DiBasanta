using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GarageManagement.API.Helpers;

public class EmailHelper
{
    private readonly IConfiguration _configuration;

    public EmailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var smtpHost = emailSettings["SmtpHost"] ?? string.Empty;
        var smtpPort = int.TryParse(emailSettings["SmtpPort"], out var parsedPort) ? parsedPort : 587;
        var senderEmail = emailSettings["SenderEmail"] ?? string.Empty;
        var senderPassword = emailSettings["SenderPassword"] ?? string.Empty;
        var senderName = emailSettings["SenderName"] ?? string.Empty;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(senderEmail, senderPassword);
        await smtpClient.SendAsync(message);
        await smtpClient.DisconnectAsync(true);
    }
}