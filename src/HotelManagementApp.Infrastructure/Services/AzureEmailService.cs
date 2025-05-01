using HotelManagementApp.Core.Interfaces.Services;
using Azure;
using Azure.Communication.Email;

namespace HotelManagementApp.Infrastructure.Services;

public class AzureEmailService() : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        var connString = Environment.GetEnvironmentVariable("AzureEmailConnectionString") ?? string.Empty;
        var emailClient = new EmailClient(connString);
        var emailDomain = Environment.GetEnvironmentVariable("AzureEmailDomain") ?? string.Empty;

        var emailMessage = new EmailMessage(
            senderAddress: $"DoNotReply@{emailDomain}",
            content: new EmailContent(subject)
            {
                Html = body
            },
        recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(to) }));
        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
        WaitUntil.Completed,
            emailMessage);
    }
}
