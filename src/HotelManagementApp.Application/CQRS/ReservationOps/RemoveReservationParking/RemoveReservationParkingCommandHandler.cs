using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationParking;

public class RemoveReservationParkingCommandHandler(
    IReservationRepository reservationRepository, 
    IReservationParkingRepository reservationParkingRepository,
    IHotelParkingRepository hotelParkingRepository) : IRequestHandler<RemoveReservationParkingCommand>
{
    public async Task Handle(RemoveReservationParkingCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        _ = await hotelParkingRepository.GetHotelParkingById(request.ParkingId, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.ParkingId} not found");
        foreach (var reservationParking in reservation.ReservationParkings)
        {
            if (reservationParking.HotelParking.Id == request.ParkingId)
            {
                await reservationParkingRepository.RemoveReservationParking(reservationParking, cancellationToken);
                break;
            }
        }
        
    }
}