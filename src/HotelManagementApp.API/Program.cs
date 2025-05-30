using DotNetEnv;
using HotelManagementApp.Application;
using HotelManagementApp.Infrastructure;

namespace HotelManagementApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);
        
        builder
                .AddApiServices()
                .AddInfrastructureServices()
                .AddApplicationServices();

        var app = builder.Build();

        app.UseAppMiddleware();

        app.Run();
    }
}
