using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ChangePassword;

public class ChangePasswordCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userManager.FindByIdAsync(request.UserId) ?? throw new UnauthorizedAccessException();
        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result)
            throw new UnauthorizedAccessException();
        await logger.Log(OperationEnum.PasswordChange, user);
    }
}
