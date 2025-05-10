using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationService;

public class RemoveReservationServiceCommand : IRequest
{
    public required int ReservationId { get; set; }
    public required int ServiceId { get; set; }
}