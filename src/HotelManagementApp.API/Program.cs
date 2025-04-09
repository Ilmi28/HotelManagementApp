
using HotelManagementApp.API.ExtensionMethods;
using HotelManagementApp.API.Middleware;
using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelManagementApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            ConfigureMiddleware(app);

            app.Run();
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenWithAuthorizationHeader();
            builder.Services.AddRepositories();
            builder.Services.AddAuthenticationWithJwt(builder.Configuration);
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext(builder.Configuration);
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly));
            builder.Services.AddServices();
            builder.Services.AddIdentity();
            builder.Services.AddLoggers();
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
        }

        public static void ConfigureMiddleware(WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }

}
