using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ChangePassword;

public class ChangePasswordCommandHandler(
    IUserManager userManager,
    IAccountDbLogger logger) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userManager.FindByIdAsync(request.UserId) 
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result)
            throw new UnauthorizedAccessException("Invalid password");
        await logger.Log(AccountOperationEnum.PasswordChange, user);
    }
}
