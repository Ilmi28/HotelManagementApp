using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.Remove;

public class RemoveFromBlacklistCommand : IRequest
{
    public required string UserId { get; set; }
}
