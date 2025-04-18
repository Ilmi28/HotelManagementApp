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

namespace HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;

public class DeleteWithoutPasswordCommandHandler(IUserManager userManager,
    IDbLogger<UserDto, AccountOperationEnum, UserLog> logger,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService) : IRequestHandler<DeleteWithoutPasswordCommand>
{
    public async Task Handle(DeleteWithoutPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var loggedInUser = authenticationService.GetLoggedInUser()
            ?? throw new UnauthorizedAccessException();
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        if (hierarchyPolicy.Succeeded)
        {
            var result = await userManager.DeleteAsync(user);
            if (!result)
                throw new Exception("User deletion failed");
            await logger.Log(AccountOperationEnum.Delete, user);

        }
        else
            throw new PolicyForbiddenException("You don't have permission to delete this account");
    }
}
