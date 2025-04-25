using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Delete;

public class DeleteAccountCommandHandler(
    IUserManager userManager,
    IAccountDbLogger logger) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            throw new UnauthorizedAccessException("Invalid password");
        result = await userManager.DeleteAsync(user);
        if (!result)
            throw new Exception("User deletion failed");
        await logger.Log(AccountOperationEnum.Delete, user);
    }
}
