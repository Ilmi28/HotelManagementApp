using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromBlacklist;

public class RemoveFromBlacklistCommandHandler(IBlacklistedUserRepository blacklistedUserRepository, IUserManager userManager) : IRequestHandler<RemoveFromBlacklistCommand>
{
    private readonly IBlacklistedUserRepository _blacklistedUserRepository = blacklistedUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(RemoveFromBlacklistCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");
        var isUserBlacklisted = await _blacklistedUserRepository.IsUserBlacklisted(request.UserId);
        if (!isUserBlacklisted)
            throw new BlackListConflictException("User is not blacklisted");
        await _blacklistedUserRepository.RemoveUserFromBlacklist(request.UserId);
    }
}
