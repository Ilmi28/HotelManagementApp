using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetCompletedOrders;

public class GetCompletedOrdersQueryHandler(
    ICompletedOrderRepository completedOrderRepository,
    IOrderStatusService orderStatusService,
    IOrderRepository orderRepository,
    IPaymentRepository paymentRepository) : IRequestHandler<GetCompletedOrdersQuery, ICollection<OrderResponse>>
{
    public async Task<ICollection<OrderResponse>> Handle(GetCompletedOrdersQuery request, CancellationToken cancellationToken)
    {
        var completedOrders = await completedOrderRepository.GetCompletedOrders(cancellationToken);
        var response = new List<OrderResponse>();
        
        foreach (var completedOrder in completedOrders)
        {
            var order = await orderRepository.GetOrderById(completedOrder.Order.Id, cancellationToken);
            if (order == null) continue;
            
            var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
            if (orderStatuses.CancelledDate != null) continue;
            
            var payment = await paymentRepository.GetPaymentsByOrderId(order.Id, cancellationToken)
                          ?? throw new PaymentNotFoundException($"Payment for order with id {order.Id} not found");
            
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
                Completed = orderStatuses?.CompletedDate,
                TotalPrice = payment.Amount
            });
        }

        return response;
    }
}