using MediatR;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Application.Responses.AuthResponses;

namespace HotelManagementApp.Application.CQRS.Auth.LoginUser
{
    public class LoginUserCommandHandler(ITokenService tokenManager,
                                    ITokenRepository tokenRepository,
                                    IUserManager userManager,
                                    IDbLogger<UserDto> logger) : IRequestHandler<LoginUserCommand, LoginRegisterResponse>
    {
        private async Task<bool> CreateRefreshTokenInDb(string hashRefreshToken, UserDto userDto)
        {
            var token = new Core.Models.RefreshToken
            {
                UserId = userDto.Id,
                RefreshTokenHash = hashRefreshToken,
                ExpirationDate = DateTime.Now.AddDays(tokenManager.GetRefreshTokenExpirationDays())
            };
            var user = await userManager.FindByIdAsync(userDto.Id);
            if (userDto == null)
                return false;
            var lastToken = await tokenRepository.GetLastValidToken(userDto.Id);
            if (lastToken != null)
                await tokenRepository.RevokeToken(lastToken);
            await tokenRepository.AddToken(token);
            return true;
        }

        public async Task<LoginRegisterResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException();
            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                throw new UnauthorizedAccessException();
            string identityToken = tokenManager.GenerateIdentityToken(user);
            string refreshToken = tokenManager.GenerateRefreshToken();
            string hashRefreshToken = tokenManager.GetHashRefreshToken(refreshToken)
                ?? throw new Exception("Refresh token creation failed");
            result = await CreateRefreshTokenInDb(hashRefreshToken, user);
            if (!result)
                throw new Exception("Refresh token creation failed");
            await logger.Log(OperationEnum.Login, user);
            return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
        }
    }
}
