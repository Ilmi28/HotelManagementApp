using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetPendingOrders;

public class GetPendingOrdersQuery : IRequest<ICollection<OrderResponse>>
{
    
}