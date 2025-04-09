using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromVIP;

public class RemoveFromVIPCommandHandler(IVIPUserRepository vipUserRepository, IUserManager userManager) : IRequestHandler<RemoveFromVIPCommand>
{
    private readonly IVIPUserRepository _vipUserRepository = vipUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(RemoveFromVIPCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException();
        var isUserVIP = await _vipUserRepository.IsUserVIP(request.UserId);
        if (!isUserVIP)
            throw new VIPConflictException("User is not a VIP");
        await _vipUserRepository.RemoveUserFromVIP(request.UserId);
    }
}
