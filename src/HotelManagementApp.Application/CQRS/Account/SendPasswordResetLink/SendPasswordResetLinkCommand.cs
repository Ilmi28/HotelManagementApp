using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink;

public class SendPasswordResetLinkCommand : IRequest
{
    public required string Email { get; set; }
}
