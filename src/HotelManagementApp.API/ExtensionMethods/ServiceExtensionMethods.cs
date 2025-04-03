﻿using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Infrastructure.Database.Context;
using HotelManagementApp.Infrastructure.Database.Identity;
using HotelManagementApp.Infrastructure.Loggers;
using HotelManagementApp.Infrastructure.Repositories;
using HotelManagementApp.Infrastructure.TokenManagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelManagementApp.API.ExtensionMethods
{
    public static class ServiceExtensionMethods
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ITokenRepository, TokenRepository>();
        }
        public static void AddTokens(this IServiceCollection services)
        {
            services.AddTransient<ITokenManager, JwtTokenManager>();
        }

        public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var tokenConfiguration = configuration!.GetSection("TokenConfiguration");
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
        }

        public static void AddLoggers(this IServiceCollection services)
        {
            services.AddTransient<IDbLogger<UserDto>, AuthDbLogger>();
        }

    }
}
