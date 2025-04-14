using MediatR;

namespace HotelManagementApp.Application.CQRS.Role.Remove;

public class RemoveFromRoleCommand : IRequest
{
    public required string UserId { get; set; }
    public required string Role { get; set; }
}
