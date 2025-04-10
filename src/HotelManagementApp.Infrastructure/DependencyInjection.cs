using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Infrastructure.Database.Context;
using HotelManagementApp.Infrastructure.Database.Identity;
using HotelManagementApp.Infrastructure.Loggers;
using HotelManagementApp.Infrastructure.Repositories;
using HotelManagementApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelManagementApp.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "HotelManagementApp.Infrastructure", "Database", "Sqlite", "hotel.db");
        var fullPath = Path.GetFullPath(dbPath);
        builder.Services.AddDbContext<HotelManagementAppDbContext>(options => options.UseSqlite($"Data source={fullPath}"));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<HotelManagementAppDbContext>();

        builder.Services.AddTransient<IUserManager, UserManager>();
        builder.Services.AddTransient<IRoleManager, RoleManager>();

        builder.Services.AddTransient<IDbLogger<UserDto>, AuthDbLogger>();

        builder.Services.AddAuthentication().AddJwtBearer(options =>
        {
            var tokenConfiguration = configuration!.GetSection("JwtTokenConfiguration");
            string secretKey = tokenConfiguration.GetValue<string>("SecretKey") ?? string.Empty;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenConfiguration.GetValue<string>("Issuer"),
                ValidAudience = tokenConfiguration.GetValue<string>("Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

        builder.Services.AddTransient<ITokenRepository, RefreshTokenRepository>();
        builder.Services.AddTransient<IVIPUserRepository, VIPUserRepository>();
        builder.Services.AddTransient<IBlacklistedUserRepository, BlacklistedUserRepository>();

        builder.Services.AddTransient<ITokenService, JwtTokenService>();
        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

        return builder;
    }
}
