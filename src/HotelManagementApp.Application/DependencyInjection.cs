using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Services;
using HotelManagementApp.Application.Services.DiscountServices;
using HotelManagementApp.Application.Services.PriceServices;
using HotelManagementApp.Application.Services.PriceServices.ReservationPriceHandlers;
using HotelManagementApp.Core.Interfaces.Handlers;
using HotelManagementApp.Core.Interfaces.OrderPricePipeline;
using HotelManagementApp.Core.Interfaces.Services;

namespace HotelManagementApp.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddScoped<IReservationPriceHandler, ReservationParkingPriceHandler>();
        builder.Services.AddScoped<IReservationPriceHandler, ReservationRoomPriceHandler>();
        builder.Services.AddScoped<IReservationPriceHandler, ReservationServicePriceHandler>();
        builder.Services.AddScoped<IReservationPricePipeline, ReservationPricePipeline>();
        
        builder.Services.AddScoped<IRoomDiscountService, RoomDiscountService>();
        builder.Services.AddScoped<IParkingDiscountService, ParkingDiscountService>();
        builder.Services.AddScoped<IServiceDiscountService, ServiceDiscountService>();
        builder.Services.AddScoped<IPricingService, OrderPricingService>();
        builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
        
        return builder;
    }
}
