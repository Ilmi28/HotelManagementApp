using MediatR;

namespace HotelManagementApp.Application.Events.OrderCompleted;

public class OrderCompletedEvent : INotification
{
    public required int OrderId { get; set; }
}