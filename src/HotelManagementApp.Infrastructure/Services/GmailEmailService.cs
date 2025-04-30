using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HotelManagementApp.Infrastructure.Services;

public class GmailEmailService(IConfiguration cfg) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        string privateKey = cfg["GmailServiceAccount:private_key"] ?? string.Empty;
        string clientEmail = cfg["GmailServiceAccount:client_email"] ?? string.Empty;

        // Build GoogleCredential from the JSON string
        var credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(clientEmail)
            {
                Scopes = new[] { GmailService.Scope.GmailSend }
            }.FromPrivateKey(privateKey)
        );

        var service = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Trywago"
        });

        // 3. Build the MIME email
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse("trywago23@gmail.com"));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        // 4. Encode it for the Gmail API
        using var stream = new MemoryStream();
        await message.WriteToAsync(stream, ct);
        string encodedMessage = Convert.ToBase64String(stream.ToArray())
                                  .Replace("+", "-")
                                  .Replace("/", "_")
                                  .Replace("=", "");

        // 5. Send it
        var gmailMessage = new Google.Apis.Gmail.v1.Data.Message { Raw = encodedMessage };
        await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync(ct);
    }
}
