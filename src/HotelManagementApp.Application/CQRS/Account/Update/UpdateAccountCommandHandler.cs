using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Update;

public class UpdateAccountCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException();

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
