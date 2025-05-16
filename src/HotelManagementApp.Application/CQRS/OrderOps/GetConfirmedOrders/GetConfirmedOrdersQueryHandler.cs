using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetConfirmedOrders;

public class GetConfirmedOrdersQueryHandler(
    IConfirmedOrderRepository confirmedOrderRepository,
    IOrderStatusService orderStatusService,
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
            
            var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
            if (orderStatuses.CancelledDate != null) continue;
            if (orderStatuses.CompletedDate != null) continue;
            
            
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
                Cancelled = null,
                Completed = null,
                TotalPrice = await pricingService.CalculatePriceForOrder(order, cancellationToken)
            });
        }

        return response;
    }
}