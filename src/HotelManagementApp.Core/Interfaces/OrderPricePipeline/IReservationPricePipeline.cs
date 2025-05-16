using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.OrderPricePipeline;

public interface IReservationPricePipeline
{
    Task<decimal> ApplyAllPrices(decimal price, Reservation reservation, CancellationToken ct);
}