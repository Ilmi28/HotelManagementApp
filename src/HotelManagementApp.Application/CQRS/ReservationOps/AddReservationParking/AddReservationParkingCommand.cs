using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;

public class AddReservationParkingCommand : IRequest
{
    public required int ReservationId { get; set; }
    public required int ParkingId { get; set; }
}