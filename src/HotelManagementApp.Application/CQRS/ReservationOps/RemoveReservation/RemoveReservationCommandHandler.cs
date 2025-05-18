using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservation;

public class RemoveReservationCommandHandler(
    IReservationRepository reservationRepository,
    IReservationParkingRepository reservationParkingRepository,
    IReservationServiceRepository reservationServiceRepository) : IRequestHandler<RemoveReservationCommand>
{
    public async Task Handle(RemoveReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        if (reservation.Order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {reservation.Order.Id} is cancelled or confirmed. You can't modify a reservation for it.");
        await reservationRepository.DeleteReservation(reservation, cancellationToken);
        foreach (var reservationParking in reservation.ReservationParkings)
            await reservationParkingRepository.RemoveReservationParking(reservationParking, cancellationToken);
        foreach (var reservationService in reservation.ReservationServices)
            await reservationServiceRepository.RemoveReservationService(reservationService, cancellationToken);
    }
}