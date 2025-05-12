using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetCancelledOrders;

public class GetCancelledOrdersQuery : IRequest<ICollection<OrderResponse>>
{
    
}