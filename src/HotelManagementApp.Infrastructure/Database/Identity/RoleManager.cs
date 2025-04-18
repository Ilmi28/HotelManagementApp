﻿using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Database.Identity;

public class RoleManager(RoleManager<IdentityRole> roleManager) : IRoleManager
{
    public async Task<List<string>> GetAllRolesAsync()
    {
        var identityRoles = await roleManager.Roles.ToListAsync();
        var result = new List<string>();
        foreach (var role in identityRoles)
            result.Add(role.Name!);
        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var identityRoles = await roleManager.Roles.ToListAsync();
        var result = new List<string>();
        foreach (var role in identityRoles)
            result.Add(role.Name!);
        return result.Contains(roleName.Normalize());
    }
}
