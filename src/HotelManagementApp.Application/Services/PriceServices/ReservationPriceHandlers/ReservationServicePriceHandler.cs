using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Handlers;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices.ReservationPriceHandlers;

public class ReservationServicePriceHandler(
    IReservationServiceRepository reservationServiceRepository,
    IHotelServiceRepository hotelServiceRepository,
    IServiceDiscountService serviceDiscountService) : IReservationPriceHandler
{
    public async Task<decimal> CalculatePrice(decimal price, Reservation reservation, CancellationToken ct)
    {
        var reservationServices = await reservationServiceRepository.GetReservationServicesByReservationId(reservation.Id, ct);
        foreach (var service in reservationServices)
        {
            var serviceModel = await hotelServiceRepository.GetHotelServiceById(service.HotelService.Id, ct)
                               ?? throw new HotelServiceNotFoundException($"Hotel service with id {service.Id} not found");
            var serviceDiscount = await serviceDiscountService.CalculateDiscount(serviceModel, ct);
            price += serviceModel.Price - (serviceModel.Price * serviceDiscount / 100m);
        }
        return price;
    }
}