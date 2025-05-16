using HotelManagementApp.Application.Dtos;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Services;

public class OrderStatusService(
    IPendingOrderRepository pendingOrderRepository,
    ICompletedOrderRepository completedOrderRepository,
    IConfirmedOrderRepository confirmedOrderRepository,
    ICancelledOrderRepository cancelledOrderRepository) : IOrderStatusService
{
    public async Task<OrderStatusesDto> GetOrderStatusesAsync(Order order, CancellationToken cancellationToken)
    {
        var pendingOrder = await pendingOrderRepository.GetPendingOrderById(order.Id, cancellationToken);
        var confirmedOrder = await confirmedOrderRepository.GetConfirmedOrderByOrderId(order.Id, cancellationToken);
        var cancelledOrder = await cancelledOrderRepository.GetCancelledOrderByOrderId(order.Id, cancellationToken);
        var completedOrder = await completedOrderRepository.GetCompletedOrderByOrderId(order.Id, cancellationToken);
        var orderStatus = new OrderStatusesDto
        {
            OrderId = order.Id,
            CreatedDate = pendingOrder?.Date,
            ConfirmedDate = confirmedOrder?.Date,
            CancelledDate = cancelledOrder?.Date,
            CompletedDate = completedOrder?.Date,
        };
        return orderStatus;
    }
}