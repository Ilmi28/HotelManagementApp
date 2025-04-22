using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Infrastructure.Database;
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

        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.GetValue<string>("SQLiteDbPath") ?? String.Empty);
        var fullPath = Path.GetFullPath(dbPath);
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data source={fullPath}"));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddTransient<IUserManager, UserManager>();
        builder.Services.AddTransient<IRoleManager, RoleManager>();
        builder.Services.AddTransient<IUserRolesManager, UserManager>();

        builder.Services.AddTransient<IAccountDbLogger, AuthDbLogger>();

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
                ValidAudiences = tokenConfiguration.GetSection("Audience").Get<string[]>(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

        builder.Services.AddTransient<ITokenRepository, RefreshTokenRepository>();
        builder.Services.AddTransient<IVIPRepository, VIPRepository>();
        builder.Services.AddTransient<IBlacklistRepository, BlacklistRepository>();
        builder.Services.AddTransient<IProfilePictureRepository, ProfilePictureRepository>();
        builder.Services.AddTransient<IHotelRepository, HotelRepository>();
        builder.Services.AddTransient<IRoomRepository, RoomRepository>();

        builder.Services.AddTransient<ITokenService, JwtTokenService>();
        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
        builder.Services.AddTransient<IImageService, ImageService>();

        return builder;
    }
}
