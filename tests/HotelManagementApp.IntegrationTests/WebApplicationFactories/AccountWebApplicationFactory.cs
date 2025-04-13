using HotelManagementApp.API;
using HotelManagementApp.Infrastructure.Database.Context;
using HotelManagementApp.IntegrationTests.TestContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelManagementApp.IntegrationTests.WebApplicationFactories;

public class AccountWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
                               options.UseInMemoryDatabase("AccountTestDb"));

            services.AddScoped<AppDbContext, TestDbContext>();
        });
    }
}
