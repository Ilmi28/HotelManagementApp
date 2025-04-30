using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelManagementApp.API.Policies.RoleHierarchyPolicy;

public class RoleHierarchyHandler(IUserManager userManager) : AuthorizationHandler<RoleHierarchyRequirement, string>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RoleHierarchyRequirement requirement, 
        string targetUserId)
    {
        var userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value).ToList();
        var targetUser = await userManager.FindByIdAsync(targetUserId);
        if (targetUser == null)
        {
            context.Fail();
            return;
        }
        var targetUserRoles = targetUser.Roles ?? new List<string>();

        var userRoleLevel = GetMaxRoleLevel(userRoles);
        var targetUserRoleLevel = GetMaxRoleLevel(targetUserRoles.ToList());

        if (userRoleLevel > targetUserRoleLevel || userRoleLevel == 3)
            context.Succeed(requirement);
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
