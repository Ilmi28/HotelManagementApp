using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Handlers;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services.PriceServices.ReservationPriceHandlers;

public class ReservationRoomPriceHandler(
    IRoomRepository roomRepository,
    IRoomDiscountService roomDiscountService) : IReservationPriceHandler
{
    public async Task<decimal> CalculatePrice(decimal price, Reservation reservation, CancellationToken ct)
    {
        var room = await roomRepository.GetRoomById(reservation.Room.Id, ct)
            ?? throw new RoomNotFoundException($"Room with id {reservation.Room.Id} not found");
        var roomDiscount = await roomDiscountService.CalculateDiscount(room, ct);
        price += room.Price - (room.Price * roomDiscount / 100m);
        return price;
    }
}