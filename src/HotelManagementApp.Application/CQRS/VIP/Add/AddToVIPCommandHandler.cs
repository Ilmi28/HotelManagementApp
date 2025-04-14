using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.Add;

public class AddToVIPCommandHandler(IVIPRepository vipUserRepository, 
                                    IUserManager userManager,
                                    IUserRolesManager userRolesManager) : IRequestHandler<AddToVIPCommand>
{
    public async Task Handle(AddToVIPCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _ = await userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException("User not found");
        var isGuest = await userRolesManager.IsUserInRoleAsync(request.UserId, "Guest");
        if (!isGuest)
            throw new PolicyForbiddenException("User is not a guest");
        var isUserVIP = await vipUserRepository.IsUserVIP(request.UserId);
        if (isUserVIP)
            throw new VIPConflictException("User is already a VIP");
        await vipUserRepository.AddUserToVIP(request.UserId);
    }
}
