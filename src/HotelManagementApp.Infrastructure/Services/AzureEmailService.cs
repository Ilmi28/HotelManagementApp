using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Infrastructure.Services;

public class AzureEmailService() : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        var connString = Environment.GetEnvironmentVariable("AzureEmailConnectionString") ?? string.Empty;
        throw new NotImplementedException();
    }
}
