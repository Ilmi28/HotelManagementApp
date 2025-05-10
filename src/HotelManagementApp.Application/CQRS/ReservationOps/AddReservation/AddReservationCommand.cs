using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;

public class AddReservationCommand : IRequest
{
    public required int OrderId { get; set; }
    public required int RoomId { get; set; }
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
}
