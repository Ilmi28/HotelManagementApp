using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using HotelManagementApp.Application.Responses.AuthResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<ITokenService> _mockTokenManager;
        private readonly Mock<ITokenRepository> _mockTokenRepository;
        private readonly Mock<IUserManager> _mockUserManager;
        private readonly Mock<IDbLogger<UserDto>> _mockLogger;
        private readonly IRequestHandler<RegisterUserCommand, LoginRegisterResponse> _handler;
        public RegisterUserCommandHandlerTests()
        {
            _mockTokenManager = new Mock<ITokenService>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockUserManager = new Mock<IUserManager>();
            _mockLogger = new Mock<IDbLogger<UserDto>>();
            _handler = new RegisterUserCommandHandler(_mockUserManager.Object, _mockTokenManager.Object, 
                                                        _mockTokenRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ValidCommand_ReturnsTokens()
        {
            var cmd = new RegisterUserCommand
            {
                Email = "test@mail.com",
                UserName = "test",
                Password = "Password123@"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), cmd.Password)).ReturnsAsync(true);
            _mockTokenManager.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(30);
            _mockTokenManager.Setup(x => x.GenerateIdentityToken(It.IsAny<UserDto>())).Returns("identityToken");
            _mockTokenManager.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
            _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashedRefreshToken");

            var response = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("identityToken", response.IdentityToken);
            Assert.Equal("refreshToken", response.RefreshToken);
        }

        [InlineData("test@mail.com", "test1")]
        [InlineData("test1@mail.com", "test")]
        [Theory]
        public async Task UserExists_ThrowsUserAlreadyExistsException(string? email, string? username)
        {
            var cmd = new RegisterUserCommand
            {
                Email = email!,
                UserName = username!,
                Password = "Password123@"
            };
            var userDto = new UserDto
            {
                Id = "1",
                Email = email!,
                UserName = username!,
                Roles = new List<string> { "Client" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.FindByEmailAsync("test1@mail.com")).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.FindByNameAsync("test1")).ReturnsAsync((UserDto?)null);

            var func = async () => await _handler.Handle(cmd, CancellationToken.None);

            await Assert.ThrowsAsync<UserExistsException>(func);
        }

        [Fact]
        public async Task NullArg_ThrowsArgumentNullException()
        {
            var func = async () => await _handler.Handle(null!, CancellationToken.None);
            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

    }
}
