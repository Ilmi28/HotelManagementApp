using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;

public class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository,
    IPendingOrderRepository pendingOrderRepository,
    ICompletedOrderRepository completedOrderRepository,
    IConfirmedOrderRepository confirmedOrderRepository,
    ICancelledOrderRepository cancelledOrderRepository,
    IPricingService pricingService) : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with id {request.OrderId} not found");

        var pendingOrder = await pendingOrderRepository.GetPendingOrderById(order.Id, cancellationToken);
        var confirmedOrder = await confirmedOrderRepository.GetConfirmedOrderByOrderId(order.Id, cancellationToken);
        var cancelledOrder = await cancelledOrderRepository.GetCancelledOrderByOrderId(order.Id, cancellationToken);
        var completedOrder = await completedOrderRepository.GetCompletedOrderByOrderId(order.Id, cancellationToken);

        return new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            Status = order.Status.ToString(),
            Address = order.OrderDetails.Address,
            City = order.OrderDetails.City,
            Country = order.OrderDetails.Country,
            FirstName = order.OrderDetails.FirstName,
            LastName = order.OrderDetails.LastName,
            PhoneNumber = order.OrderDetails.PhoneNumber,
            Created = pendingOrder?.Date,
            Confirmed = confirmedOrder?.Date,
            Canceled = cancelledOrder?.Date,
            Completed = completedOrder?.Date,
            TotalPrice = await pricingService.CalculatePriceForOrder(order, cancellationToken)
        };
    }
}