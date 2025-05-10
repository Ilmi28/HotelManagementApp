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
        
        builder.WebHost.UseUrls("http://*:5000");
        
        builder
                .AddApiServices()
                .AddInfrastructureServices()
                .AddApplicationServices();

        var app = builder.Build();

        app.UseAppMiddleware();

        app.Run();
    }
}
