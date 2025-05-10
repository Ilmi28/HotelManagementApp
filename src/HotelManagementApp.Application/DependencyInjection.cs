using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using HotelManagementApp.Application.Services;
using HotelManagementApp.Core.Interfaces.Services;

namespace HotelManagementApp.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddScoped<IRoomDiscountService, RoomDiscountService>();
        builder.Services.AddScoped<IParkingDiscountService, ParkingDiscountService>();
        builder.Services.AddScoped<IServiceDiscountService, ServiceDiscountService>();
        
        return builder;
    }
}
