using HotelManagementApp.Core.Interfaces.Handlers;
using HotelManagementApp.Core.Interfaces.OrderPricePipeline;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices;

public class ReservationPricePipeline(IEnumerable<IReservationPriceHandler> handlers) : IReservationPricePipeline
{
    public async Task<decimal> ApplyAllPrices(decimal price, Reservation reservation, CancellationToken ct)
    {
        foreach (var handler in handlers)
            price = await handler.CalculatePrice(price, reservation, ct);
        return price;
    }
}