using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices;

public class BillProductService(
    IReservationRepository reservationRepository,
    IOrderBillProductRepository billProductRepository,
    IHotelParkingRepository hotelParkingRepository,
    IHotelServiceRepository hotelServiceRepository,
    IRoomRepository roomRepository,
    IPricingService pricingService) : IBillProductService
{
    public async Task AddBillProductsForOrder(Order order, CancellationToken ct)
    {
        foreach (var reservation in order.Reservations)
        {
            var reservationModel = await reservationRepository.GetReservationById(reservation.Id, ct)
                ?? throw new ReservationNotFoundException($"Reservation with ID {order.Id} not found");
            var room = await roomRepository.GetRoomById(reservationModel.Room.Id, ct)
                ?? throw new RoomNotFoundException($"Room with id {reservation.Room.Id} not found");
            await AddParkingBillProducts(reservationModel, room.Hotel, ct);
            await AddServiceBillProducts(reservationModel, room.Hotel, ct);
            await AddRoomBillProduct(reservationModel, room.Hotel, ct);
        }
    }

    private async Task AddParkingBillProducts(Reservation reservation, Hotel hotel, CancellationToken ct)
    {
        foreach (var parking in reservation.ReservationParkings)
        {
            var parkingModel = await hotelParkingRepository.GetHotelParkingById(parking.HotelParking.Id, ct)
                               ?? throw new HotelParkingNotFoundException($"Parking with id {parking.HotelParking.Id} not found");
            var billProduct = new OrderBillProduct
            {
                Name = $"Parking - {hotel.Name} - {parking.HotelParking.CarSpaces} miejsc parkingowych",
                Price = await pricingService.CalculatePriceForParking(parkingModel, ct),
                OrderId = reservation.Order.Id,
                Quantity = parking.Quantity,
            };
            await billProductRepository.AddOrderBillProduct(billProduct, ct);
        }
    }

    private async Task AddServiceBillProducts(Reservation reservation, Hotel hotel, CancellationToken ct)
    {
        foreach (var service in reservation.ReservationServices)
        {
            var serviceModel = await hotelServiceRepository.GetHotelServiceById(service.HotelService.Id, ct)
                ?? throw new HotelServiceNotFoundException($"Service with id {service.HotelService.Id} not found");
            var billProduct = new OrderBillProduct
            {
                Name = $"Usługa - {hotel.Name} - {service.HotelService.Name}",
                Price = await pricingService.CalculatePriceForService(serviceModel, ct),
                OrderId = reservation.Order.Id,
                Quantity = service.Quantity,
            };
            await billProductRepository.AddOrderBillProduct(billProduct, ct);
        }
    }

    private async Task AddRoomBillProduct(Reservation reservation, Hotel hotel, CancellationToken ct)
    {
        var room = await roomRepository.GetRoomById(reservation.Room.Id, ct)
            ?? throw new RoomNotFoundException($"Room with id {reservation.Room.Id} not found");
        var days = (reservation.To.DayNumber - reservation.From.DayNumber) + 1;
        var roomPrice = await pricingService.CalculatePriceForRoom(room, ct);
        var billProduct = new OrderBillProduct
        {
            Name = $"Pokój {room.RoomName} - {room.RoomType} - {hotel.Name} - na {days} dzień/dni",
            OrderId = reservation.Order.Id,
            Price = roomPrice * days,
        };
        await billProductRepository.AddOrderBillProduct(billProduct, ct);
    }
}