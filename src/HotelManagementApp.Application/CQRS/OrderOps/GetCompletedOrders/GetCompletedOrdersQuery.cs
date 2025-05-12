using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetCompletedOrders;

public class GetCompletedOrdersQuery : IRequest<ICollection<OrderResponse>>
{
    
}