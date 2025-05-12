using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HotelManagementApp.Core.Interfaces.Identity;

namespace HotelManagementApp.API.Policies.ConfirmedEmailPolicy;

public class ConfirmedEmailHandler(IUserManager userManager) : AuthorizationHandler<ConfirmedEmailRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ConfirmedEmailRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId!);
        if (user is not null && user.IsEmailConfirmed == true)
            context.Succeed(requirement);
    }
}