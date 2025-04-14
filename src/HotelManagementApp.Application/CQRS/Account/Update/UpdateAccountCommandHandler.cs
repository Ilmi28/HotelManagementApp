using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.Account.Update;

public class UpdateAccountCommandHandler(
    IUserManager userManager, 
    IDbLogger<UserDto> logger,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userManager.FindByIdAsync(request.UserId) 
            ?? throw new UnauthorizedAccessException();
        var loggedInUser = authenticationService.GetLoggedInUser()
            ?? throw new UnauthorizedAccessException();
        var authorizationResult = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        if (!authorizationResult.Succeeded)
            throw new RoleForbiddenException("You do not have permission to update this user.");
        var userByUserName = await userManager.FindByNameAsync(request.UserName);
        if (userByUserName != null && userByUserName.Id != request.UserId)
            throw new UserExistsException("User with this username already exists.");
        user.UserName = request.UserName;

        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail != null && userByEmail.Id != request.UserId)
            throw new UserExistsException("User with this email already exists.");
        user.Email = request.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result)
            throw new Exception("User update failed");
        await logger.Log(AccountOperationEnum.Update, user);

    }
}
