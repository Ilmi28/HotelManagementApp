using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Delete;

public class DeleteAccountCommand : IRequest
{
    public required string UserId { get; set; }
    public required string Password { get; set; }
}
