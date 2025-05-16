using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;

public class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository,
    IOrderStatusService orderStatusService,
    IPricingService pricingService,
    IPaymentRepository paymentRepository) : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with id {request.OrderId} not found");

        var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
        var payment = await paymentRepository.GetPaymentsByOrderId(order.Id, cancellationToken);
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
            Created = orderStatuses.CreatedDate,
            Confirmed = orderStatuses.ConfirmedDate,
            Cancelled = orderStatuses.CancelledDate,
            Completed = orderStatuses.CompletedDate,
            TotalPrice = payment?.Amount ?? await pricingService.CalculatePriceForOrder(order, cancellationToken),
        };
    }
}