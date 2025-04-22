using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.VIP.IsGuestVIP;

public class IsGuestVIPQueryHandler(
    IUserManager userManager, 
    IVIPRepository vipRepository,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService) 
    : IRequestHandler<IsGuestVIPQuery, bool>
{
    public async Task<bool> Handle(IsGuestVIPQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new UnauthorizedAccessException();
        var loggedInUser = authenticationService.GetLoggedInUser();
        if (loggedInUser is null)
            throw new UnauthorizedAccessException();
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        if (hierarchyPolicy.Succeeded || ownerPolicy.Succeeded)
        {
            var isVIP = await vipRepository.IsUserVIP(user.Id, cancellationToken);
            return isVIP;
        }
        else
            throw new UnauthorizedAccessException();
    }
}
