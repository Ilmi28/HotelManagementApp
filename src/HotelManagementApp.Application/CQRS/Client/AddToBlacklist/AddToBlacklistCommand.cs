using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.AddToBlacklist;

public class AddToBlacklistCommand : IRequest
{
    public required string UserId { get; set; }
}
