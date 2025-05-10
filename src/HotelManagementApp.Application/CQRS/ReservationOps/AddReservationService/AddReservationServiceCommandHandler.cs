using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservationService;

public class AddReservationServiceCommandHandler(
    IReservationRepository reservationRepository,
    IHotelServiceRepository hotelServiceRepository,
    IReservationServiceRepository reservationServiceRepository,
    IRoomRepository roomRepository) : IRequestHandler<AddReservationServiceCommand>
{
    public async Task Handle(AddReservationServiceCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        var service = await hotelServiceRepository.GetHotelServiceById(request.ServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.ServiceId} not found");
        var room = await roomRepository.GetRoomById(reservation.Room.Id, cancellationToken) 
            ?? throw new RoomNotFoundException($"Room with id {reservation.Room.Id} not found");
        if (service.Hotel.Id != room.Hotel.Id)
            throw new InvalidOperationException("Hotel service and reservation must be from the same hotel");
        var reservationService = new ReservationService
        {
            Reservation = reservation,
            HotelService = service
        };
        await reservationServiceRepository.AddReservationService(reservationService, cancellationToken);
    }
}