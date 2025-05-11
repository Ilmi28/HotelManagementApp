using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services;

public class PricingService(
    IParkingDiscountService parkingDiscountService, 
    IRoomDiscountService roomDiscountService, 
    IServiceDiscountService serviceDiscountService,
    IReservationRepository reservationRepository,
    IReservationParkingRepository reservationParkingRepository,
    IReservationServiceRepository reservationServiceRepository,
    IHotelParkingRepository hotelParkingRepository,
    IHotelServiceRepository hotelServiceRepository,
    IRoomRepository roomRepository) : IPricingService
{
    public async Task<decimal> CalculatePriceForOrder(Order order, CancellationToken ct)
    {
        var totalPrice = 0m;
        foreach (var reservation in order.Reservations)
        {
            var reservationModel = await reservationRepository.GetReservationById(reservation.Id)
                ?? throw new ReservationNotFoundException($"Reservation with id {reservation.Id} not found");
            var room = await roomRepository.GetRoomById(reservationModel.Room.Id, ct)
                ?? throw new RoomNotFoundException($"Room with id {reservationModel.Room.Id} not found");
            var roomDiscount = await roomDiscountService.CalculateDiscount(room, ct);
            var reservationParkings = await reservationParkingRepository.GetReservationParkingsByReservationId(reservation.Id, ct);
            var reservationServices = await reservationServiceRepository.GetReservationServicesByReservationId(reservation.Id, ct);
            foreach (var parking in reservationParkings)
            {
                var parkingModel = await hotelParkingRepository.GetHotelParkingById(parking.Id, ct)
                    ?? throw new HotelParkingNotFoundException($"Hotel parking with id {parking.Id} not found");
                var parkingDiscount = await parkingDiscountService.CalculateDiscount(parkingModel, ct);
                totalPrice += parkingModel.Price - (parkingModel.Price * parkingDiscount / 100m);
            }

            foreach (var service in reservationServices)
            {
                var serviceModel = await hotelServiceRepository.GetHotelServiceById(service.Id, ct)
                    ?? throw new HotelServiceNotFoundException($"Hotel service with id {service.Id} not found");
                var serviceDiscount = await serviceDiscountService.CalculateDiscount(serviceModel, ct);
                totalPrice += serviceModel.Price - (serviceModel.Price * serviceDiscount / 100m);
            }
            totalPrice += room.Price - (room.Price * roomDiscount / 100m);
        }
        return totalPrice;
    }
}