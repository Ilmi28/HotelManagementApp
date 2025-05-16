using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Handlers;

public interface IReservationPriceHandler
{
    Task<decimal> CalculatePrice(decimal price, Reservation reservation, CancellationToken ct);
}