using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetAvailableDays;

public class GetAvailableDaysQuery : IRequest<ICollection<DateOnly>>
{
    public required int RoomId { get; set; }
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
}