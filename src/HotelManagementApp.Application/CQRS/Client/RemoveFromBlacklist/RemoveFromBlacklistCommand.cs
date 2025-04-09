using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromBlacklist;

public class RemoveFromBlacklistCommand : IRequest
{
    public required string UserId { get; set; }
}
