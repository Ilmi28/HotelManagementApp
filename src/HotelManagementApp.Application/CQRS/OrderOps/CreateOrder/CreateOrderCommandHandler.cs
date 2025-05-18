using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;

public class CreateOrderCommandHandler(
    IUserManager userManager, 
    IOrderRepository orderRepository,
    IPendingOrderRepository pendingOrderRepository) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        if (!user.Roles.Contains("Guest"))
            throw new InvalidOperationException("Guest role is required to create an order");
        var orderDetails = new OrderDetails
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
        };
        var order = new Order
        {
            UserId = request.UserId,
            OrderDetails = orderDetails,
            Status = OrderStatusEnum.Pending
        };
        orderDetails.Order = order;
        await orderRepository.AddOrder(order, cancellationToken);
        await pendingOrderRepository.AddPendingOrder(new PendingOrder {Order = order}, cancellationToken);
        return order.Id;
    }
}
