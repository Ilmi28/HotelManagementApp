using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;

public class GetOrdersByGuestQuery : IRequest<ICollection<OrderResponse>>
{
    public required string GuestId { get; set; }
}