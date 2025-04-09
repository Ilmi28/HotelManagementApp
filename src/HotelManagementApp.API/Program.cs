using HotelManagementApp.API.ExtensionMethods;
using HotelManagementApp.Application;
using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Infrastructure;

namespace HotelManagementApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
                .AddApiServices()
                .AddInfrastructureServices()
                .AddApplicationServices();

        var app = builder.Build();

        app.UseAppMiddleware();

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
        builder.Services.AddProblemDetails();
    }

    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseAppExceptionHandler();
        app.UseStatusCodePages();
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
