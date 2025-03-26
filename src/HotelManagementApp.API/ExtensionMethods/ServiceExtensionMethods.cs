using HotelManagementApp.Infrastructure.Database;
using HotelManagementApp.Infrastructure.Database.Identity;
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
            // Add repositories here
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Add services here
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

        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HotelManagementAppDbContext>(options => options.UseSqlServer(connString));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HotelManagementAppDbContext>();
        }
    }
}
