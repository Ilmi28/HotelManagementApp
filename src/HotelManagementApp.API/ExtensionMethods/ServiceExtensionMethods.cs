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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HotelManagementApp.API.ExtensionMethods;

public static class ServiceExtensionMethods
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ITokenRepository, RefreshTokenRepository>();
        services.AddTransient<IVIPUserRepository, VIPUserRepository>();
        services.AddTransient<IBlacklistedUserRepository, BlacklistedUserRepository>();
    }
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, JwtTokenService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
    }

    public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddJwtBearer(options =>
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
    }

    public static void AddSwaggerGenWithAuthorizationHeader(this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            var security = new OpenApiSecurityScheme
            {
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "JWT Authorization header",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            x.AddSecurityDefinition(security.Reference.Id, security);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{security, Array.Empty<string>()}});
        });
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "HotelManagementApp.Infrastructure", "Database", "Sqlite", "hotel.db");
        var fullPath = Path.GetFullPath(dbPath);
        services.AddDbContext<HotelManagementAppDbContext>(options => options.UseSqlite($"Data source={fullPath}"));
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HotelManagementAppDbContext>();
        services.AddTransient<IUserManager, UserManager>();
        services.AddTransient<IRoleManager, RoleManager>();
    }

    public static void AddLoggers(this IServiceCollection services)
    {
        services.AddTransient<IDbLogger<UserDto>, AuthDbLogger>();
    }

}
