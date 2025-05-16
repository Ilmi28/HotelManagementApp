using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;

public class GetOrdersByGuestQueryHandler(
    IOrderRepository orderRepository,
    IUserManager userManager,
    IPricingService pricingService,
    IPaymentRepository paymentRepository,
    IOrderStatusService orderStatusService) : IRequestHandler<GetOrdersByGuestQuery, ICollection<OrderResponse>>
{
    public async Task<ICollection<OrderResponse>> Handle(GetOrdersByGuestQuery request, CancellationToken cancellationToken)
    {
        
        var user = await userManager.FindByIdAsync(request.GuestId)
            ?? throw new UserNotFoundException($"User with id {request.GuestId} not found");
        if (!user.Roles.Contains("Guest"))
            throw new InvalidOperationException($"User with id {user.Id} is not a guest");
        var orders = await orderRepository.GetOrdersByGuestId(request.GuestId, cancellationToken);
        var response = new List<OrderResponse>();
        foreach (var order in orders)
        {
            var orderStatuses = await orderStatusService.GetOrderStatusesAsync(order, cancellationToken);
            var payment = await paymentRepository.GetPaymentsByOrderId(order.Id, cancellationToken);
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
                Created = orderStatuses.CreatedDate,
                Confirmed = orderStatuses.ConfirmedDate,
                Cancelled = orderStatuses.CancelledDate,
                Completed = orderStatuses.CompletedDate,
                TotalPrice = payment?.Amount ?? await pricingService.CalculatePriceForOrder(order, cancellationToken)
            });
        }

        return response;

    }
}