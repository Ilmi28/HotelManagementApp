using HotelManagementApp.Application.Responses.AuthResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RegisterUser;

public class RegisterUserCommandHandler(IUserManager userManager,
                                    ITokenService tokenManager,
                                    ITokenRepository tokenRepository,
                                    IDbLogger<UserDto> logger) : IRequestHandler<RegisterUserCommand, LoginRegisterResponse>
{
    private async Task CreateRefreshTokenInDb(string hashRefreshToken, UserDto user)
    {
        var token = new Core.Models.RefreshToken
        {
            UserId = user.Id,
            RefreshTokenHash = hashRefreshToken,
            ExpirationDate = DateTime.Now.AddDays(tokenManager.GetRefreshTokenExpirationDays())
        };
        await tokenRepository.AddToken(token);
    }

    public async Task<LoginRegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        var userByName = await userManager.FindByNameAsync(request.UserName);
        if (userByEmail != null)
            throw new UserExistsException("User with this email already exists.");
        if (userByName != null)
            throw new UserExistsException("User with this username already exists.");
        var user = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            Roles = ["Guest"]
        };
        await userManager.CreateAsync(user, request.Password);
        string identityToken = tokenManager.GenerateIdentityToken(user);
        string refreshToken = tokenManager.GenerateRefreshToken();
        string hashRefreshToken = tokenManager.GetHashRefreshToken(refreshToken)
            ?? throw new Exception("Refresh token creation failed");
        await CreateRefreshTokenInDb(hashRefreshToken, user);
        await logger.Log(AccountOperationEnum.Register, user);
        return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
    }
}
