using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Exceptions.NotFound;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.UpdateOrder;

public class UpdateOrderCommandHandler(
    IUserManager userManager, 
    IOrderRepository orderRepository) : IRequestHandler<UpdateOrderCommand>
{
    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        if (!user.Roles.Contains("Guest"))
            throw new InvalidOperationException("Guest role is required to update an order");
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with ID {request.OrderId} not found");
        if (order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Completed or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order should be pending to change details. Current status: {order.Status}");
        order.OrderDetails.FirstName = request.FirstName;
        order.OrderDetails.LastName = request.LastName;
        order.OrderDetails.PhoneNumber = request.PhoneNumber;
        order.OrderDetails.Address = request.Address;
        order.OrderDetails.City = request.City;
        order.OrderDetails.Country = request.Country;
        
        await orderRepository.UpdateOrder(order, cancellationToken);
    }
}