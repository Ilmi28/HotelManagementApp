using HotelManagementApp.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelManagementApp.Application.Policies.AccountOwnerPolicy;

public class AccountOwnerHandler : AuthorizationHandler<AccountOwnerRequirement, UserDto>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        AccountOwnerRequirement requirement, UserDto resource)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (userId == resource.Id)
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
