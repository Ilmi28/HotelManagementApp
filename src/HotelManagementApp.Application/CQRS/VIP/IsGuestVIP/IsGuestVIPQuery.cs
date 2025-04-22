using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.IsGuestVIP;

public class IsGuestVIPQuery : IRequest<bool>
{
    public required string UserId { get; set; }
}
