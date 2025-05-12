using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.CancelOrder;

public class CancelOrderCommand : IRequest
{
    public required int OrderId { get; set; }
}