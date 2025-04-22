using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<ITokenService> _mockTokenManager;
    private readonly Mock<ITokenRepository> _mockTokenRepository;
    private readonly Mock<IUserManager> _mockUserManager;
    private readonly Mock<IAccountDbLogger> _mockLogger;
    private readonly LoginUserCommandHandler _handler;
    public LoginUserCommandHandlerTests()
    {
        _mockTokenManager = new Mock<ITokenService>();
        _mockTokenRepository = new Mock<ITokenRepository>();
        _mockUserManager = new Mock<IUserManager>();
        _mockLogger = new Mock<IAccountDbLogger>();
        _handler = new LoginUserCommandHandler(_mockTokenManager.Object, _mockTokenRepository.Object,
                                                _mockUserManager.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ValidCommand_ReturnsIdentityToken()
    {
        var cmd = new LoginUserCommand
        {
            Email = "test@mail.com",
            Password = "Password123@"
        };
        var userDto = new UserDto
        {
            Id = "1",
            Email = "test@mail.com",
            UserName = "test",
            Roles = ["Client"]
        };

        _mockTokenManager.Setup(x => x.GetRefreshTokenExpirationDays()).Returns(30);
        _mockTokenManager.Setup(x => x.GenerateIdentityToken(userDto)).Returns("identityToken");
        _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(userDto);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(userDto, "Password123@")).ReturnsAsync(true);
        _mockTokenManager.Setup(x => x.GenerateIdentityToken(userDto)).Returns("identityToken");
        _mockTokenManager.Setup(x => x.GenerateRefreshToken()).Returns("refreshToken");
        _mockTokenManager.Setup(x => x.GetHashRefreshToken(It.IsAny<string>())).Returns("hashedRefreshToken");

        var response = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal("identityToken", response.IdentityToken);
        Assert.Equal("refreshToken", response.RefreshToken);
    }

    [InlineData("test@mail.com", "InvalidPassword")]
    [InlineData("test1@mail.com", "Password123@")]
    [InlineData(null!, "Password123@")]
    [InlineData("test@mail.com", null!)]
    [Theory]
    public async Task InvalidEmailOrPassword_ThrowsUnauthorizedException(string? email, string? password)
    {
        var cmd = new LoginUserCommand
        {
            Email = email!,
            Password = password!
        };
        var userDto = new UserDto
        {
            Id = "1",
            Email = "test@mail.com",
            UserName = "test",
            Roles = ["Client"]
        };
        _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(userDto);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<UserDto>(), "InvalidPassword")).ReturnsAsync(false);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<UserDto>(), "Password123@")).ReturnsAsync(true);

        var func = async () => await _handler.Handle(cmd, CancellationToken.None);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(func);
    }

    [Fact]
    public async Task NullArg_ThrowsArgumentNullException()
    {
        LoginUserCommand cmd = null!;
        var func = async () => await _handler.Handle(cmd, CancellationToken.None);
        await Assert.ThrowsAsync<ArgumentNullException>(func);
    }
}
