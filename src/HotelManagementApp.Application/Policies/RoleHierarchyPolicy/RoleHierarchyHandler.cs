using HotelManagementApp.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelManagementApp.Application.Policies.RoleHierarchyPolicy;

public class RoleHierarchyHandler : AuthorizationHandler<RoleHierarchyRequirement, UserDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RoleHierarchyRequirement requirement, 
        UserDto resource)
    {
        var userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value).ToList();
        var targetUserRoles = resource.Roles.ToList();

        var userRoleLevel = GetMaxRoleLevel(userRoles);
        var targetUserRoleLevel = GetMaxRoleLevel(targetUserRoles);

        if (userRoleLevel > targetUserRoleLevel)
            context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private int GetMaxRoleLevel(List<string> roles)
    {
        var hierarchyLevels = new Dictionary<string, int>
        {
            { "Admin", 3 },
            { "Manager", 2 },
            { "Staff", 1 },
            { "Guest", 0 }
        };

        int maxRoleLevel = 0;
        foreach (var role in roles)
            maxRoleLevel = Math.Max(hierarchyLevels[role], maxRoleLevel);
        return maxRoleLevel;
    }
}
