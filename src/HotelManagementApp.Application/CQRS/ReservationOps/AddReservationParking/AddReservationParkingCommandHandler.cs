using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;

public class AddReservationParkingCommandHandler(
    IReservationRepository reservationRepository, 
    IHotelParkingRepository hotelParkingRepository,
    IReservationParkingRepository reservationParkingRepository,
    IRoomRepository roomRepository) : IRequestHandler<AddReservationParkingCommand>
{
    public async Task Handle(AddReservationParkingCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        var parking = await hotelParkingRepository.GetHotelParkingById(request.ParkingId, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.ParkingId} not found");
        var room = await roomRepository.GetRoomById(reservation.Room.Id, cancellationToken) 
            ?? throw new RoomNotFoundException($"Room with id {reservation.Room.Id} not found");
        if (reservation.Order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {reservation.Order.Id} is cancelled or confirmed. You can't modify a reservation for it.");
        if (parking.Hotel.Id != room.Hotel.Id)
            throw new InvalidOperationException("Hotel parking and reservation must be from the same hotel");
        var reservationParking = new ReservationParking
        {
            Reservation = reservation,
            HotelParking = parking,
            Quantity = request.Quantity
        };
        await reservationParkingRepository.AddReservationParking(reservationParking, cancellationToken);
    }
}