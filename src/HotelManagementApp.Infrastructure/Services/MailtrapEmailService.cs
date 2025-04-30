using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using MediatR;

public class MailtrapEmailService(IConfiguration config, HttpClient httpClient) : IEmailService
{
    public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config["Mailtrap:ApiKey"]);
        var payload = new
        {
            from = new { email = fromEmail, name = "Trywago" },
            to = new[] { new { email = toEmail } },
            subject = subject,
            text = body
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://sandbox.api.mailtrap.io/api/send/3649397", content);

        response.EnsureSuccessStatusCode(); 
    }

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
