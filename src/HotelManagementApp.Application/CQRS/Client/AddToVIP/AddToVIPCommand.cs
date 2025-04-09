using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.AddToVIP;

public class AddToVIPCommand : IRequest
{
    public required string UserId { get; set; }
}
