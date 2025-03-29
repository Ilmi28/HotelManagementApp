using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Application.Services;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Exceptions.UserExceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.UnitTests.ServiceTests
{
    public class AuthServiceTests
    {
        private Mock<ITokenManager> _mockTokenManager;
        private Mock<ITokenRepository> _mockTokenRepository;
        private Mock<IUserManager> _mockUserManager;
        private Mock<IDbLogger<UserDto>> _mockLogger;
        private IAuthService _authService;
        public AuthServiceTests()
        {
            _mockTokenManager = new Mock<ITokenManager>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockUserManager = new Mock<IUserManager>();
            _mockLogger = new Mock<IDbLogger<UserDto>>();
            _authService = new AuthService(_mockTokenManager.Object, _mockTokenRepository.Object, _mockUserManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task LoginUser_ValidEmailAndPassword_ReturnsTokens()
        {
            var userDto = new UserDto
            {
                Id = "1",
                UserName = "test",
                Email = "test@gmail.com",
                Roles = new List<string> { "User" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<UserDto>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockTokenManager.Setup(x => x.GenerateAccessToken(It.IsAny<UserDto>())).Returns("identityToken");
            _mockTokenManager.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashRefreshToken");
            _mockTokenManager.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(30);
            _mockTokenRepository.Setup(x => x.AddToken(It.IsAny<Token>())).ReturnsAsync(true);

           (string identityToken, string refreshToken) = await _authService.LoginUser("test@gmail.com", "Password123@");

            Assert.Equal("identityToken", identityToken);
            Assert.Equal("refreshToken", refreshToken);
        }

        [Fact]
        public async Task LoginUser_InvalidEmail_ThrowsUnauthorizedAccessException()
        {
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserDto?)null);

            var func = async () => await _authService.LoginUser("test@gmail.com", "Password123@");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(func);
        }

        [Fact]
        public async Task Loginuser_ValidEmailAndInvalidPassword_ThrowsUnauthorizedAccessException()
        {
            var userDto = new UserDto
            {
                Id = "1",
                Email = "test@gmail.com",
                UserName = "test",
                Roles = new List<string> { "User" }
            };
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<UserDto>(), It.IsAny<string>())).ReturnsAsync(false);

            var func = async () => await _authService.LoginUser("test@gmail.com", "Password123@");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(func);
        }

        [InlineData("test@gmail.com", null!)]
        [InlineData(null!, "Password123@")]
        [Theory]
        public async Task LoginUser_NullArgs_ThrowsArgumentNullException(string? email, string? password)
        {
            var func = async () => await _authService.LoginUser(email!, password!);

            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

        [Fact]
        public async Task LoginUser_DatabaseError_ThrowsDatabaseErrorException()
        {
            var userDto = new UserDto
            {
                Id = "1",
                UserName = "test",
                Email = "test@gmail.com",
                Roles = new List<string> { "User" }
            };
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<UserDto>(), It.IsAny<string>())).Throws(new Exception());

            var func = async () => await _authService.LoginUser("test@gmail.com", "Password123@");

            await Assert.ThrowsAsync<DatabaseErrorException>(func);
        }

        [Fact]
        public async Task RegisterUser_ValidArgs_ReturnsTokens()
        {
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockTokenManager.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashRefreshToken");
            _mockTokenManager.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(30);
            _mockTokenManager.Setup(x => x.GenerateAccessToken(It.IsAny<UserDto>())).Returns("identityToken");
            _mockTokenRepository.Setup(x => x.AddToken(It.IsAny<Token>())).ReturnsAsync(true);

            var roles = new List<string> { "User" };
            (string identityToken, string refreshToken) = await _authService.RegisterUser("test", "test@gmail.com", "Password123@", roles);

            Assert.Equal("identityToken", identityToken);
            Assert.Equal("refreshToken", refreshToken);
        }

        [Fact]
        public async Task RegisterUser_UserWithEmailExists_ThrowsUserWithEmailAlreadyExistsException()
        {
            var userDto = new UserDto
            {
                Id = "1",
                UserName = "test",
                Email = "test@gmail.com",
                Roles = new List<string> { "User" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((UserDto?)null);

            var roles = new List<string> { "User" };
            var func = async () => await _authService.RegisterUser("test", "test@gmail.com", "Password123@", roles);

            await Assert.ThrowsAsync<UserWithEmailAlreadyExistsException>(func);
        }

        [Fact]
        public async Task RegisterUser_UserWithUserNameExists_ThrowsUserWithUserNameAlreadyExistsException()
        {
            var userDto = new UserDto
            {
                Id = "1",
                UserName = "test",
                Email = "test@gmail.com",
                Roles = new List<string> { "User" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(userDto);

            var roles = new List<string> { "User" };
            var func = async () => await _authService.RegisterUser("test", "test@gmail.com", "Password123@", roles);

            await Assert.ThrowsAsync<UserWithUserNameAlreadyExistsException>(func);
        }

        [InlineData(null!, "test@gmail.com", "Password123@")]
        [InlineData("test", null!, "Password123@")]
        [InlineData("test", "test@gmail.com", null!)]
        [Theory]
        public async Task RegisterUser_NullArgs_ThrowsArgumentNullException(string? username, string? email, string? password)
        {
            var roles = new List<string> { "User" };
            var func = async () => await _authService.RegisterUser(username!, email!, password!, roles);

            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

        [Fact]
        public async Task RegisterUser_DatabaseError_ThrowsDatabaseErrorException()
        {
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Throws(new Exception());

            var roles = new List<string> { "User" };
            var func = async () => await _authService.RegisterUser("test", "test@gmail.com", "Password123@", roles);

            await Assert.ThrowsAsync<DatabaseErrorException>(func);
        }

        [Fact]
        public async Task RefreshToken_ValidRefreshToken_ReturnsIdentityToken()
        {
            var userDto = new UserDto
            {
                Id = "1",
                UserName = "test",
                Email = "test@gmail.com",
                Roles = new List<string> { "User" }
            };
            var token = new Token
            {
                Id = 1,
                RefreshTokenHash = "hashRefreshToken",
                UserId = "1",
                ExpirationDate = DateTime.Now.AddDays(30)
            };
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashRefreshToken");
            _mockTokenRepository.Setup(x => x.GetToken(It.IsAny<string>())).ReturnsAsync(token);
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDto);
            _mockTokenManager.Setup(x => x.GenerateAccessToken(It.IsAny<UserDto>())).Returns("identityToken");

            var response = await _authService.RefreshToken("refreshToken");

            Assert.Equal("identityToken", response);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ThrowsUnauthorizedAccessException()
        {
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashRefreshToken");
            _mockTokenRepository.Setup(x => x.GetToken(It.IsAny<string>())).ReturnsAsync((Token?)null);

            var func = async () => await _authService.RefreshToken("refreshToken");
            await Assert.ThrowsAsync<UnauthorizedAccessException>(func);
        }

        [Fact]
        public async Task RefreshToken_NullArg_ThrowsArgumentNullException()
        {
            var func = async () => await _authService.RefreshToken(null!);
            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

        [Fact]
        public async Task RefreshToken_DatabaseError_ThrowsDatabaseErrorException()
        {
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Throws(new Exception());
            var func = async () => await _authService.RefreshToken("refreshToken");
            await Assert.ThrowsAsync<DatabaseErrorException>(func);
        }

    }
}
