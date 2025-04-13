using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.Remove;

public class RemoveFromVIPCommand : IRequest
{
    public required string UserId { get; set; }
}
