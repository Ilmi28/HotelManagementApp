using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.Application.CQRS.Account.ChangePassword;

public class ChangePasswordCommandHandler(
    IUserManager userManager,
    IAccountDbLogger logger,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userManager.FindByIdAsync(request.UserId) 
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var loggedInUser = authenticationService.GetLoggedInUser()
            ?? throw new UnauthorizedAccessException();
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        if (ownerPolicy.Succeeded)
        {
            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result)
                throw new UnauthorizedAccessException("Invalid password");
            await logger.Log(AccountOperationEnum.PasswordChange, user);
        }
        else
            throw new PolicyForbiddenException("You don't have permission to change this account's password");
    }
}
