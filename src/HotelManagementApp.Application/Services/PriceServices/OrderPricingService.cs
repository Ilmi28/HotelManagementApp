using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.OrderPricePipeline;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices;

public class OrderPricingService(
    IReservationRepository reservationRepository,
    IParkingDiscountService parkingDiscountService,
    IServiceDiscountService serviceDiscountService,
    IRoomDiscountService roomDiscountService,
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

    public async Task<decimal> CalculatePriceForParking(HotelParking hotelParking, CancellationToken ct)
    {
        var parkingDiscount = await parkingDiscountService.CalculateDiscount(hotelParking, ct);
        return hotelParking.Price - (hotelParking.Price * parkingDiscount / 100m);
    }

    public async Task<decimal> CalculatePriceForRoom(HotelRoom hotelRoom, CancellationToken ct)
    {
        var roomDiscount = await roomDiscountService.CalculateDiscount(hotelRoom, ct);
        return hotelRoom.Price - (roomDiscount * roomDiscount / 100m);
    }

    public async Task<decimal> CalculatePriceForService(HotelService hotelService, CancellationToken ct)
    {
        var serviceDiscount = await serviceDiscountService.CalculateDiscount(hotelService, ct);
        return hotelService.Price - (serviceDiscount * serviceDiscount / 100m);
    }
}