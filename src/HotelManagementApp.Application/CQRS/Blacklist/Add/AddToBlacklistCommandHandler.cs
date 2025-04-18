using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.Add;

public class AddToBlacklistCommandHandler(IBlacklistRepository blacklistedUserRepository, 
                                            IUserManager userManager,
                                            IUserRolesManager userRolesManager) : IRequestHandler<AddToBlacklistCommand>
{
    private readonly IBlacklistRepository _blacklistedUserRepository = blacklistedUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(AddToBlacklistCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _ = await _userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException("User not found");
        var isGuest = await userRolesManager.IsUserInRoleAsync(request.UserId, "Guest");
        if (!isGuest)
            throw new PolicyForbiddenException("User is not a guest");
        var isUserBlacklisted = await _blacklistedUserRepository.IsUserBlacklisted(request.UserId, cancellationToken);
        if (isUserBlacklisted)
            throw new BlackListConflictException("User is already blacklisted");
        await _blacklistedUserRepository.AddUserToBlacklist(request.UserId, cancellationToken);
    }
}
