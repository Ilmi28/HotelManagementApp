using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;

public class DeleteWithoutPasswordCommand : IRequest
{
    public required string UserId { get; set; }
}
