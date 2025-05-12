using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetConfirmedOrders;

public class GetConfirmedOrdersQuery : IRequest<ICollection<OrderResponse>>
{
    
}