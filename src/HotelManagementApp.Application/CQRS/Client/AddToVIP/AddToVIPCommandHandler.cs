using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Client.AddToVIP;

public class AddToVIPCommandHandler(IVIPUserRepository vipUserRepository, IUserManager userManager) : IRequestHandler<AddToVIPCommand>
{
    private readonly IUserManager _userManager = userManager;
    private readonly IVIPUserRepository _vipUserRepository = vipUserRepository;

    public async Task Handle(AddToVIPCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");
        var isUserVIP = await _vipUserRepository.IsUserVIP(request.UserId);
        if (isUserVIP)
            throw new VIPConflictException("User is already a VIP");
        await _vipUserRepository.AddUserToVIP(request.UserId);
    }
}
