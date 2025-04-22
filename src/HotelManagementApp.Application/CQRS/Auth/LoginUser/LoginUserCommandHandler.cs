using HotelManagementApp.Application.Responses.AuthResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.LoginUser;

public class LoginUserCommandHandler(ITokenService tokenManager,
                                ITokenRepository tokenRepository,
                                IUserManager userManager,
                                IAccountDbLogger logger) : IRequestHandler<LoginUserCommand, LoginRegisterResponse>
{
    private async Task<bool> CreateRefreshTokenInDb(string hashRefreshToken, UserDto userDto, CancellationToken ct)
    {
        var token = new RefreshToken
        {
            UserId = userDto.Id,
            RefreshTokenHash = hashRefreshToken,
            ExpirationDate = DateTime.Now.AddDays(tokenManager.GetRefreshTokenExpirationDays())
        };
        _ = await userManager.FindByIdAsync(userDto.Id);
        if (userDto == null)
            return false;
        var lastToken = await tokenRepository.GetLastValidToken(userDto.Id, ct);
        if (lastToken != null)
            await tokenRepository.RevokeToken(lastToken, ct);
        await tokenRepository.AddToken(token, ct);
        return true;
    }

    public async Task<LoginRegisterResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var user = await userManager.FindByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException();
        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            throw new UnauthorizedAccessException();
        string identityToken = tokenManager.GenerateIdentityToken(user);
        string refreshToken = tokenManager.GenerateRefreshToken();
        string hashRefreshToken = tokenManager.GetHashRefreshToken(refreshToken)
            ?? throw new Exception("Refresh token creation failed");
        result = await CreateRefreshTokenInDb(hashRefreshToken, user, cancellationToken);
        if (!result)
            throw new Exception("Refresh token creation failed");
        await logger.Log(AccountOperationEnum.Login, user);
        return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
    }
}
