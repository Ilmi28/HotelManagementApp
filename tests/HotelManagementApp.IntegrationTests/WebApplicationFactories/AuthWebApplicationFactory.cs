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

public class AuthWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<HotelManagementAppDbContext>>();
            services.RemoveAll<HotelManagementAppDbContext>();

            services.AddDbContext<HotelManagementAppDbContext>(options =>
                               options.UseInMemoryDatabase("AuthTestDb"));

            services.AddScoped<HotelManagementAppDbContext, TestDbContext>();
        });
    }
}
