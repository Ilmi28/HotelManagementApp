using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetCancelledOrders;

public class GetCancelledOrdersQueryHandler(
    ICancelledOrderRepository cancelledOrderRepository,
    IOrderStatusService orderStatusService,
    IOrderRepository orderRepository,
    IPricingService pricingService) : IRequestHandler<GetCancelledOrdersQuery, ICollection<OrderResponse>>
{
    public async Task<ICollection<OrderResponse>> Handle(GetCancelledOrdersQuery request, CancellationToken cancellationToken)
    {
        var cancelledOrders = await cancelledOrderRepository.GetCancelledOrders(cancellationToken);
        var response = new List<OrderResponse>();
        
        foreach (var cancelledOrder in cancelledOrders)
        {
            var order = await orderRepository.GetOrderById(cancelledOrder.Order.Id, cancellationToken);
            if (order == null) continue;
            
            var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
            
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
                Confirmed = orderStatuses?.ConfirmedDate,
                Cancelled = orderStatuses?.CancelledDate,
                Completed = orderStatuses?.CompletedDate,
                TotalPrice = await pricingService.CalculatePriceForOrder(order, cancellationToken)
            });
        }

        return response;
    }
}