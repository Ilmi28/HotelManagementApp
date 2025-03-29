
using HotelManagementApp.API.ExtensionMethods;
using HotelManagementApp.API.Middleware;
using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

            app.MapControllers();
            app.Run();
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddJwtBearer(builder.Configuration);
            builder.Services.AddDbContext(builder.Configuration);
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly));
            builder.Services.AddTokens();
            builder.Services.AddIdentity();
            builder.Services.AddLoggers();
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
            app.UseAuthorization();
        }
    }

}
