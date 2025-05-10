using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetReservationParkings;

public class GetReservationParkingsQuery : IRequest<ICollection<ReservationParkingResponse>>
{
    public required int ReservationId { get; set; }
}