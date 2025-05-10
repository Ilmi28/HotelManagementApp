using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrderReservations;

public class GetOrderReservationsQuery : IRequest<ICollection<ReservationResponse>>
{
    public required int OrderId { get; set; }
}