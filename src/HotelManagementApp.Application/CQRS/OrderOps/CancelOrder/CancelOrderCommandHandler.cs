using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.CancelOrder;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository, 
    ICancelledOrderRepository cancelledOrderRepository) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
                    ?? throw new OrderNotFoundException($"Order with id: {request.OrderId} not found");
        if (order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Completed)
            throw new InvalidOperationException($"Order is already in final state. Current status: {order.Status}"); 
        order.Status = OrderStatusEnum.Cancelled;
        await orderRepository.UpdateOrder(order, cancellationToken);
        var cancelledOrder = new CancelledOrder
        {
            Order = order
        };
        await cancelledOrderRepository.AddCancelledOrder(cancelledOrder, cancellationToken);
    }
}