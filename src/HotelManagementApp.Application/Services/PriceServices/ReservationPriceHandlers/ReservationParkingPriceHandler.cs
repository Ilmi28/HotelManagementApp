using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Handlers;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices.ReservationPriceHandlers;

public class ReservationParkingPriceHandler(
    IReservationParkingRepository reservationParkingRepository,
    IHotelParkingRepository hotelParkingRepository,
    IParkingDiscountService parkingDiscountService) : IReservationPriceHandler
{
    public async Task<decimal> CalculatePrice(decimal price, Reservation reservation, CancellationToken ct)
    {
        var reservationParkings = await reservationParkingRepository.GetReservationParkingsByReservationId(reservation.Id, ct);
        foreach (var parking in reservationParkings)
        {
            var parkingModel = await hotelParkingRepository.GetHotelParkingById(parking.HotelParking.Id, ct)
                               ?? throw new HotelParkingNotFoundException($"Hotel parking with id {parking.Id} not found");
            var parkingDiscount = await parkingDiscountService.CalculateDiscount(parkingModel, ct);
            var parkingPrice = parkingModel.Price - (parkingModel.Price * parkingDiscount / 100m);
            price += parkingPrice *  parking.Quantity;
        }
        return price;
    }
}