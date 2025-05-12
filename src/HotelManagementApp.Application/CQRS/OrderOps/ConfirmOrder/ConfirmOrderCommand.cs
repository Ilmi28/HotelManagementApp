using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.ConfirmOrder;

public class ConfirmOrderCommand : IRequest
{
    public required int OrderId { get; set; }
}