using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.Add;

public class AddToBlacklistCommand : IRequest
{
    public required string UserId { get; set; }
}
