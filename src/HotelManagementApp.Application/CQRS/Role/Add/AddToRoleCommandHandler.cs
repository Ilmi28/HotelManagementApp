﻿using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.Role.Add;

public class AddToRoleCommandHandler(
    IUserManager userManager, 
    IUserRolesManager userRolesManager) 
    : IRequestHandler<AddToRoleCommand>
{
    public async Task Handle(AddToRoleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException("User not found.");
        var userRoles = await userRolesManager.GetUserRolesAsync(request.UserId);
        var forbiddenRolesForGuest = new[] { "Staff", "Manager", "Admin" };
        if (!CheckForGuestRole(request.Role, userRoles))
            throw new RoleForbiddenException("Guest role cannot be assigned to personnel.");
        if (!CheckForPersonnelRole(request.Role, userRoles))
            throw new RoleForbiddenException("Personnel role cannot be assigned to guest.");
        var isInRole = await userRolesManager.IsUserInRoleAsync(request.UserId, request.Role);
        if (isInRole)
            throw new RoleConflictException("User already has this role.");
        var result = await userRolesManager.AddToRoleAsync(user, request.Role);
        if (!result)
            throw new Exception("Unexpected error occured or invalid role.");
    }

    private bool CheckForGuestRole(string role, ICollection<string> userRoles)
    {
        var forbiddenRolesForGuest = new[] { "Staff", "Manager", "Admin" };
        if (role == "Guest" && userRoles.Any(x => forbiddenRolesForGuest.Contains(x)))
            return false;
        return true;
    }

    private bool CheckForPersonnelRole(string role, ICollection<string> userRoles)
    {
        if ((role == "Staff" || role == "Manager" || role == "Admin") && userRoles.Contains("Guest"))
            return false;
        return true;
    }
}
