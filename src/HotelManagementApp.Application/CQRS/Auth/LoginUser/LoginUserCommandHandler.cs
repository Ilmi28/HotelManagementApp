using MediatR;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Responses.AuthResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Interfaces.Loggers;

namespace HotelManagementApp.Application.CQRS.Auth.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginRegisterResponse>
    {
        private ITokenManager _tokenManager;
        private ITokenRepository _tokenRepository;
        private IUserManager _userManager;
        private IDbLogger<UserDto> _logger;
        public LoginUserCommandHandler(ITokenManager tokenManager, 
                                        ITokenRepository tokenRepository,
                                        IUserManager userManager,
                                        IDbLogger<UserDto> logger)
        {
            _tokenManager = tokenManager;
            _tokenRepository = tokenRepository;
            _userManager = userManager;
            _logger = logger;
        }

        private async Task<bool> CreateRefreshTokenInDb(string hashRefreshToken, UserDto user)
        {
            var token = new Token
            {
                UserId = user.Id,
                RefreshTokenHash = hashRefreshToken,
                ExpirationDate = DateTime.Now.AddDays(_tokenManager.GetRefreshTokenExpirationDays())
            };
            return await _tokenRepository.AddToken(token);
        }

        public async Task<LoginRegisterResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    throw new UnauthorizedAccessException();
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                    throw new UnauthorizedAccessException();
                string identityToken = _tokenManager.GenerateIdentityToken(user);
                string refreshToken = _tokenManager.GenerateRefreshToken();
                string hashRefreshToken = _tokenManager.GetHashRefreshToken(refreshToken)
                    ?? throw new Exception("Refresh token creation failed");
                result = await CreateRefreshTokenInDb(hashRefreshToken, user);
                if (!result)
                    throw new Exception("Refresh token creation failed");
                await _logger.Log(Operation.Login, user);
                return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex) { throw new Exception("Unexpected error occured", ex); }
        }
    }
}
