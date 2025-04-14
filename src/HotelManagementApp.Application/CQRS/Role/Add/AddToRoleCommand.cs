using MediatR;

namespace HotelManagementApp.Application.CQRS.Role.Add;

public class AddToRoleCommand : IRequest
{
    public required string UserId { get; set; }
    public required string Role { get; set; } 
}
