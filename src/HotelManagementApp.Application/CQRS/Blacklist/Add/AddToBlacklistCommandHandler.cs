using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.Add;

public class AddToBlacklistCommandHandler(IBlacklistRepository blacklistedUserRepository, 
                                            IUserManager userManager) : IRequestHandler<AddToBlacklistCommand>
{

    public async Task Handle(AddToBlacklistCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _ = await userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException("User not found");
        var isUserBlacklisted = await blacklistedUserRepository.IsUserBlacklisted(request.UserId, cancellationToken);
        if (isUserBlacklisted)
            throw new BlackListConflictException("User is already blacklisted");
        await blacklistedUserRepository.AddUserToBlacklist(request.UserId, cancellationToken);
    }
}
