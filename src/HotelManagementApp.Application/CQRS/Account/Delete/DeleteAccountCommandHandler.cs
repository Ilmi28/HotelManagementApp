using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.Account.Delete;

public class DeleteAccountCommandHandler(
    IUserManager userManager,
    IDbLogger<UserDto, AccountOperationEnum, UserLog> logger,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var loggedInUser = authenticationService.GetLoggedInUser()
            ?? throw new UnauthorizedAccessException();
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        if (hierarchyPolicy.Succeeded || ownerPolicy.Succeeded)
        {
            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                throw new UnauthorizedAccessException("Invalid password");
            result = await userManager.DeleteAsync(user);
            if (!result)
                throw new Exception("User deletion failed");
            await logger.Log(AccountOperationEnum.Delete, user);
        }
        else
            throw new PolicyForbiddenException("You don't have permission to delete this account");
    }
}
