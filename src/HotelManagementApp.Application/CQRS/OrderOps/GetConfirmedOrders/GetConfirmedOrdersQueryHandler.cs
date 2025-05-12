using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetConfirmedOrders;

public class GetConfirmedOrdersQueryHandler(
    IConfirmedOrderRepository confirmedOrderRepository,
    IPendingOrderRepository pendingOrderRepository,
    ICompletedOrderRepository completedOrderRepository,
    ICancelledOrderRepository cancelledOrderRepository,
    IOrderRepository orderRepository,
    IPricingService pricingService) : IRequestHandler<GetConfirmedOrdersQuery, ICollection<OrderResponse>>
{
    public async Task<ICollection<OrderResponse>> Handle(GetConfirmedOrdersQuery request, CancellationToken cancellationToken)
    {
        var confirmedOrders = await confirmedOrderRepository.GetConfirmedOrders(cancellationToken);
        var response = new List<OrderResponse>();
        
        foreach (var confirmedOrder in confirmedOrders)
        {
            var order = await orderRepository.GetOrderById(confirmedOrder.Order.Id, cancellationToken);
            if (order == null) continue;
            
            var cancelledOrder = await cancelledOrderRepository.GetCancelledOrderByOrderId(order.Id, cancellationToken);
            if (cancelledOrder != null) continue;
            
            var completedOrder = await completedOrderRepository.GetCompletedOrderByOrderId(order.Id, cancellationToken);
            if (completedOrder != null) continue;
            
            var pendingOrder = await pendingOrderRepository.GetPendingOrderByOrderId(order.Id, cancellationToken);
            
            response.Add(new OrderResponse
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
                Confirmed = confirmedOrder.Date,
                Cancelled = null,
                Completed = null,
                TotalPrice = await pricingService.CalculatePriceForOrder(order, cancellationToken)
            });
        }

        return response;
    }
}