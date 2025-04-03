using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Responses.AuthResponses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, LoginRegisterResponse>
    {
        private IUserManager _userManager;
        private ITokenManager _tokenManager;
        private ITokenRepository _tokenRepository;
        private IDbLogger<UserDto> _logger;
        public RegisterUserCommandHandler(IUserManager userManager,
                                            ITokenManager tokenManager,
                                            ITokenRepository tokenRepository,
                                            IDbLogger<UserDto> logger)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        private async Task CreateRefreshTokenInDb(string hashRefreshToken, UserDto user)
        {
            var token = new Token
            {
                UserId = user.Id,
                RefreshTokenHash = hashRefreshToken,
                ExpirationDate = DateTime.Now.AddDays(_tokenManager.GetRefreshTokenExpirationDays())
            };
            await _tokenRepository.AddToken(token);
        }

        public async Task<LoginRegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            var userByName = await _userManager.FindByNameAsync(request.UserName);
            if (userByEmail != null)
                throw new UserAlreadyExistsException("User with this email already exists.");
            if (userByName != null)
                throw new UserAlreadyExistsException("User with this username already exists.");
            var user = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                Roles = new List<string>() { "Client" }
            };
            try
            {
                await _userManager.CreateAsync(user, request.Password);
                string identityToken = _tokenManager.GenerateIdentityToken(user);
                string refreshToken = _tokenManager.GenerateRefreshToken();
                string hashRefreshToken = _tokenManager.GetHashRefreshToken(refreshToken)
                    ?? throw new Exception("Refresh token creation failed");
                await CreateRefreshTokenInDb(hashRefreshToken, user);
                await _logger.Log(Operation.Register, user);
                return new LoginRegisterResponse { IdentityToken = identityToken, RefreshToken = refreshToken };
            }
            catch (Exception ex) { throw new Exception("Unexpected error occured", ex); }
        }
    }
}
