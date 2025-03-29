using HotelManagementApp.API;
using HotelManagementApp.API.ExtensionMethods;
using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Infrastructure.Database.Context;
using HotelManagementApp.IntegrationTests.TestContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.IntegrationTests.WebApplicationFactories
{
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
}
