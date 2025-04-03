using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Infrastructure.Database.Context;
using HotelManagementApp.Infrastructure.Database.Identity;
using HotelManagementApp.IntegrationTests.WebApplicationFactories;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagementApp.IntegrationTests.Tests
{
    public class AccountTests : IClassFixture<AccountWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly HotelManagementAppDbContext _context;
        public AccountTests(AccountWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<HotelManagementAppDbContext>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            SeedData();
        }

        private void SeedData()
        {

        }
    }
}
