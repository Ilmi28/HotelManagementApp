using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.Remove;

public class RemoveFromVIPCommandHandler(IVIPRepository vipUserRepository, IUserManager userManager) : IRequestHandler<RemoveFromVIPCommand>
{
    private readonly IVIPRepository _vipUserRepository = vipUserRepository;
    private readonly IUserManager _userManager = userManager;

    public async Task Handle(RemoveFromVIPCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        _ = await _userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException();
        var isUserVIP = await _vipUserRepository.IsUserVIP(request.UserId, cancellationToken);
        if (!isUserVIP)
            throw new VIPNotFoundException("User is not a VIP");
        await _vipUserRepository.RemoveUserFromVIP(request.UserId, cancellationToken);
    }
}
