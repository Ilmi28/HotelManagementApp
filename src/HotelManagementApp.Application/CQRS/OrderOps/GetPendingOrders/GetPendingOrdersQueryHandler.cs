using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetPendingOrders;

public class GetPendingOrdersQueryHandler(
    IPendingOrderRepository pendingOrderRepository,
    IOrderStatusService orderStatusService,
    IOrderRepository orderRepository,
    IPricingService pricingService) : IRequestHandler<GetPendingOrdersQuery, ICollection<OrderResponse>>
{
    public async Task<ICollection<OrderResponse>> Handle(GetPendingOrdersQuery request, CancellationToken cancellationToken)
    {
        var pendingOrders = await pendingOrderRepository.GetPendingOrders(cancellationToken);
        var response = new List<OrderResponse>();
        
        foreach (var pendingOrder in pendingOrders)
        {
            var order = await orderRepository.GetOrderById(pendingOrder.Order.Id, cancellationToken);
            if (order == null) continue;
            
            var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
            if (orderStatuses.ConfirmedDate != null) continue;
            if (orderStatuses.CancelledDate != null) continue;
            
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
                Created = orderStatuses?.CreatedDate,
                Confirmed = null,
                Cancelled = null,
                Completed = null,
                TotalPrice = await pricingService.CalculatePriceForOrder(order, cancellationToken)
            });
        }

        return response;
    }
}