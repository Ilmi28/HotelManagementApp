using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.AddToBlacklist;

public class AddToBlacklistCommandHandler(IBlacklistedUserRepository blacklistedUserRepository, IUserManager userManager) : IRequestHandler<AddToBlacklistCommand>
{
    private readonly IBlacklistedUserRepository _blacklistedUserRepository = blacklistedUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(AddToBlacklistCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");
        var isUserBlacklisted = await _blacklistedUserRepository.IsUserBlacklisted(request.UserId);
        if (isUserBlacklisted)
            throw new BlackListConflictException("User is already blacklisted");
        await _blacklistedUserRepository.AddUserToBlacklist(request.UserId);
    }
}
