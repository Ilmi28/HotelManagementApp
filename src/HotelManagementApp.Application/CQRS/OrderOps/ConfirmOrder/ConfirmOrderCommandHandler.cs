using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.ConfirmOrder;

public class ConfirmOrderCommandHandler(
    IOrderRepository orderRepository, 
    IConfirmedOrderRepository confirmedOrderRepository) : IRequestHandler<ConfirmOrderCommand>
{
    public async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
                    ?? throw new OrderNotFoundException($"Order with id: {request.OrderId} not found");
        if (order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Completed or OrderStatusEnum.Confirmed) 
            throw new InvalidOperationException($"Only pending order can be confirmed. Current status: {order.Status}");
        order.Status = OrderStatusEnum.Confirmed;
        await orderRepository.UpdateOrder(order, cancellationToken);
        var confirmedOrder = new ConfirmedOrder
        {
            Order = order
        };
        await confirmedOrderRepository.AddConfirmedOrder(confirmedOrder, cancellationToken);
    }
}