using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationParking;

public class RemoveReservationParkingCommand : IRequest
{
    public required int ReservationId { get; set; }
    public required int ParkingId { get; set; }
}