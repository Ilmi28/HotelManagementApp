using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Infrastructure.Services;

public class AzureEmailService(IConfiguration config) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
