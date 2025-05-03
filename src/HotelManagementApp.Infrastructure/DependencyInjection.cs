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

        builder.Services.AddScoped<IUserManager, UserManager>();
        builder.Services.AddScoped<IRoleManager, RoleManager>();
        builder.Services.AddScoped<IUserRolesManager, UserManager>();

        builder.Services.AddScoped<IAccountDbLogger, AuthDbLogger>();

        builder.Services.AddAuthentication().AddJwtBearer(options =>
        {
            var tokenConfiguration = configuration!.GetSection("JwtTokenConfiguration");
            string secretKey = Environment.GetEnvironmentVariable("JwtSecretKey") 
                ?? tokenConfiguration.GetValue<string>("SecretKey")
                ?? string.Empty;
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

        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IVIPRepository, VIPRepository>();
        builder.Services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        builder.Services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IRoomRepository, HotelRoomRepository>();
        builder.Services.AddScoped<IHotelImageRepository, HotelImageRepository>();
        builder.Services.AddScoped<IRoomImageRepository, RoomImageRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<IConfirmEmailTokensRepository, ConfirmEmailTokensRepository>();
        builder.Services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();
        builder.Services.AddScoped<IHotelServiceRepository, HotelServiceRepository>();
        builder.Services.AddScoped<IHotelParkingRepository, HotelParkingRepository>();

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IFileService, LocalFileService>();
        builder.Services.AddScoped<IEmailService, AzureEmailService>();
        builder.Services.AddHttpClient<ICityService, GeonamesCityService>();

        return builder;
    }
}
