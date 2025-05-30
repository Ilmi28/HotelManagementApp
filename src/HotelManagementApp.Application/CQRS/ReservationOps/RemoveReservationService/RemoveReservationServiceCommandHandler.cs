using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationService;

public class RemoveReservationServiceCommandHandler(
    IReservationRepository reservationRepository,
    IReservationServiceRepository reservationServiceRepository,
    IHotelServiceRepository hotelServiceRepository) : IRequestHandler<RemoveReservationServiceCommand>
{
    public async Task Handle(RemoveReservationServiceCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        _ = await hotelServiceRepository.GetHotelServiceById(request.ServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.ServiceId} not found");
        if (reservation.Order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {reservation.Order.Id} is cancelled or confirmed. You can't modify a reservation for it.");
        foreach (var reservationService in reservation.ReservationServices)
        {
            if (reservationService.HotelService.Id == request.ServiceId)
            {
                await reservationServiceRepository.RemoveReservationService(reservationService, cancellationToken);
                break;
            }
        }
    }
}