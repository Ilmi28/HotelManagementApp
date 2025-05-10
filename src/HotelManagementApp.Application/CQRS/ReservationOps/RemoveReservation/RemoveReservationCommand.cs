using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservation;

public class RemoveReservationCommand : IRequest
{
    public required int ReservationId { get; set; }
}