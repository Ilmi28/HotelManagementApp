using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Create;

public class CreateAccountCommandHandler(IUserManager userManager, IAccountDbLogger logger) : IRequestHandler<CreateAccountCommand, string>
{
    public async Task<string> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var dbUser = await userManager.FindByEmailAsync(request.Email);
        if (dbUser != null)
            throw new UserExistsException("User with this email already exists.");
        dbUser = await userManager.FindByNameAsync(request.UserName);
        if (dbUser != null)
            throw new UserExistsException("User with this username already exists.");
        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            Roles = request.Roles
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result)
            throw new Exception("Invalid role");
        await logger.Log(AccountOperationEnum.Create, user);
        return user.Id;
    }
}
