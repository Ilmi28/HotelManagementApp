using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Role.Remove;

public class RemoveFromRoleCommandHandler(IUserManager userManager, IUserRolesManager userRolesManager)
    : IRequestHandler<RemoveFromRoleCommand>
{
    public async Task Handle(RemoveFromRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException("User not found.");
        if (request.Role == "Guest")
            throw new RoleForbiddenException("Guest role cannot be removed.");
        var isInRole = await userRolesManager.IsUserInRoleAsync(request.UserId, request.Role);
        if (!isInRole)
            throw new RoleConflictException("User does not have this role.");
        var result = await userRolesManager.RemoveFromRoleAsync(user, request.Role);
        if (!result)
            throw new Exception("Unexpected error occured or invalid role.");
    }
}
