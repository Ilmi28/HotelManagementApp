using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromVIP;

public class RemoveFromVIPCommand : IRequest
{
    public required string UserId { get; set; }
}
