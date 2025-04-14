using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;

public class DeleteWithoutPasswordCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger) : IRequestHandler<DeleteWithoutPasswordCommand>
{
    public async Task Handle(DeleteWithoutPasswordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UserNotFoundException($"User with id {request.UserId} not found");
        var result = await userManager.DeleteAsync(user);
        if (!result)
            throw new Exception("User deletion failed");
        await logger.Log(AccountOperationEnum.Delete, user);
    }
}
