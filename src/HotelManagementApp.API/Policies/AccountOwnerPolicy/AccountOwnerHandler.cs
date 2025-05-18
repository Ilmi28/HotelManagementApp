using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelManagementApp.API.Policies.AccountOwnerPolicy;

public class AccountOwnerHandler : AuthorizationHandler<AccountOwnerRequirement, string>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        AccountOwnerRequirement requirement, string targetAccountId)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (userId == targetAccountId)
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
