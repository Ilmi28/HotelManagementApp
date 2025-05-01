using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ConfirmEmail;

public class ConfirmEmailCommand : IRequest
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}
