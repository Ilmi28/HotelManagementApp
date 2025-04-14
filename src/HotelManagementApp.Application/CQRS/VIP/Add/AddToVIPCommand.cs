using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.Add;

public class AddToVIPCommand : IRequest
{
    public required string UserId { get; set; }
}
