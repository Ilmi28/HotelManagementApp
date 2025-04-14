﻿using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.Remove;

public class RemoveFromBlacklistCommandHandler(IBlacklistRepository blacklistedUserRepository, IUserManager userManager) : IRequestHandler<RemoveFromBlacklistCommand>
{
    private readonly IBlacklistRepository _blacklistedUserRepository = blacklistedUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(RemoveFromBlacklistCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _ = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException("User not found");
        var isUserBlacklisted = await _blacklistedUserRepository.IsUserBlacklisted(request.UserId);
        if (!isUserBlacklisted)
            throw new BlacklistUserNotFound("User is not blacklisted");
        await _blacklistedUserRepository.RemoveUserFromBlacklist(request.UserId);
    }
}
