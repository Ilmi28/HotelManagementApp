using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using MediatR;

namespace HotelManagementApp.Application.Events.OrderCompleted;

public class AddLoyaltyPointsOrderCompletedHandler(
    ILoyaltyPointsRepository loyaltyPointsRepository, 
    IOrderRepository orderRepository,
    ILoyaltyPointsHistoryRepository historyRepository,
    IPricingService pricingService) : INotificationHandler<OrderCompletedEvent>
{
    public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(notification.OrderId, cancellationToken)
                    ?? throw new OrderNotFoundException($"Order with id {notification.OrderId} not found");
        var price = await pricingService.CalculatePriceForOrder(order, cancellationToken);
        var points = (int)Math.Floor(price) * 4;
        var existingLoyaltyPoints = await loyaltyPointsRepository.GetLoyaltyPointsByGuestId(order.UserId, cancellationToken);
        if (existingLoyaltyPoints == null)
        {
            var loyaltyPoints = new LoyaltyPoints
            {
                GuestId = order.UserId,
                Points = points
            };
            await loyaltyPointsRepository.AddLoyaltyPoints(loyaltyPoints, cancellationToken);
        }
        else
        {
            existingLoyaltyPoints.Points += points;
            await loyaltyPointsRepository.UpdateLoyaltyPoints(existingLoyaltyPoints, cancellationToken);
        }
        var historyLog = new LoyaltyPointsLog
        {
            UserId = order.UserId,
            Description = $"Order nr {order.Id}",
            Points = points
        };
        await historyRepository.AddPointsLog(historyLog, cancellationToken);
    }
}