using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderResponse>
{
    public required int OrderId { get; set; }
}