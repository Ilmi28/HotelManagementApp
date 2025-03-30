using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Exceptions.UserExceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private ITokenManager _tokenManager;
        private ITokenRepository _tokenRepository;
        private IUserManager _userManager;
        private IDbLogger<UserDto> _logger;
        public AuthService(ITokenManager tokenManager, ITokenRepository tokenRepository, IUserManager userManager, IDbLogger<UserDto> logger)
        {
            _tokenManager = tokenManager;
            _tokenRepository = tokenRepository;
            _userManager = userManager;
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
        public async Task<(string identityToken, string refreshToken)> LoginUser(string email, string password)
        {
            _ = password ?? throw new ArgumentNullException();
            _ = email ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new UnauthorizedAccessException();
                var result = await _userManager.CheckPasswordAsync(user, password);
                if (!result)
                    throw new UnauthorizedAccessException();
                string identityToken = _tokenManager.GenerateIdentityToken(user);
                string refreshToken = _tokenManager.GenerateRefreshToken();
                string hashRefreshToken = _tokenManager.GetHashRefreshToken(refreshToken)
                    ?? throw new UnauthorizedAccessException();
                await CreateRefreshTokenInDb(hashRefreshToken, user);
                await _logger.Log(Operation.Login, user);
                return (identityToken, refreshToken);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch { throw new DatabaseErrorException(); }
        }

        public async Task<(string identityToken, string refreshToken)> RegisterUser(string userName, string email, string password, List<string> roles)
        {
            _ = userName ?? throw new ArgumentNullException();
            _ = email ?? throw new ArgumentNullException();
            _ = password ?? throw new ArgumentNullException();
            var user = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = email,
                Roles = roles
            };
            try
            {
                var userByEmail = await _userManager.FindByEmailAsync(email);
                var userByName = await _userManager.FindByNameAsync(userName);
                if (userByEmail != null)
                    throw new UserWithEmailAlreadyExistsException();
                if (userByName != null)
                    throw new UserWithUserNameAlreadyExistsException();
                await _userManager.CreateAsync(user, password);
                string identityToken = _tokenManager.GenerateIdentityToken(user);
                string refreshToken = _tokenManager.GenerateRefreshToken();
                string hashRefreshToken = _tokenManager.GetHashRefreshToken(refreshToken)
                    ?? throw new DatabaseErrorException();
                await CreateRefreshTokenInDb(hashRefreshToken, user);
                await _logger.Log(Operation.Register, user);
                return (identityToken, refreshToken);
            }
            catch (UserAlreadyExistsException) { throw; }
            catch { throw new DatabaseErrorException(); }
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            _ = refreshToken ?? throw new ArgumentNullException();
            try
            {
                string hash = _tokenManager.GetHashRefreshToken(refreshToken)
                                                        ?? throw new UnauthorizedAccessException();
                var token = await _tokenRepository.GetToken(hash)
                                                        ?? throw new UnauthorizedAccessException();
                var user = await _userManager.FindByIdAsync(token.UserId)
                                                        ?? throw new UnauthorizedAccessException();
                var identityToken = _tokenManager.GenerateIdentityToken(user);
                return identityToken;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch
            {
                throw new DatabaseErrorException();
            }
        }
    }
}
