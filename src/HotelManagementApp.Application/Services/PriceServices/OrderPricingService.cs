using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.OrderPricePipeline;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices;

public class OrderPricingService(
    IReservationRepository reservationRepository,
    IReservationPricePipeline pipeline) : IPricingService
{
    public async Task<decimal> CalculatePriceForOrder(Order order, CancellationToken ct)
    {
        var totalPrice = 0m;
        foreach (var reservation in order.Reservations)
        {
            var reservationModel = await reservationRepository.GetReservationById(reservation.Id, ct)
                ?? throw new ReservationNotFoundException($"Reservation with id {reservation.Id} not found");
            totalPrice = await pipeline.ApplyAllPrices(totalPrice, reservationModel, ct);
        }
        return totalPrice;
    }
}