using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.IsGuestVIP;

public class IsGuestVIPQueryHandler(
    IUserManager userManager, 
    IVIPRepository vipRepository) 
    : IRequestHandler<IsGuestVIPQuery, bool>
{
    public async Task<bool> Handle(IsGuestVIPQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new UnauthorizedAccessException();
        var isVIP = await vipRepository.IsUserVIP(user.Id, cancellationToken);
        return isVIP;
    }
}
