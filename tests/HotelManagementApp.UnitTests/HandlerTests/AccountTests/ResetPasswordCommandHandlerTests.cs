using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Account.ResetPassword;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<IResetPasswordTokenRepository> _tokenRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        _userManagerMock = new Mock<IUserManager>();
        _tokenRepositoryMock = new Mock<IResetPasswordTokenRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ResetPasswordCommandHandler(_userManagerMock.Object, _tokenRepositoryMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldResetPassword_WhenTokenIsValid()
    {
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string>
            {
                "Client"
            },
            IsEmailConfirmed = false
        };
        var token = new ResetPasswordToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(10),ResetPasswordTokenHash="exampleHash" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "NewPassword")).ReturnsAsync(true);

        await _handler.Handle(new ResetPasswordCommand { UserId = "123", ResetPasswordToken = "token", NewPassword = "NewPassword" }, CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ResetPasswordCommand { UserId = "123", ResetPasswordToken = "token", NewPassword = "NewPassword" }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenTokenIsInvalid()
    {
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string>
            {
                "Client"
            }
        };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns((string?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ResetPasswordCommand { UserId = "123", ResetPasswordToken = "token", NewPassword = "NewPassword" }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenTokenIsExpired()
    {
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string>
            {
                "Client"
            },
            IsEmailConfirmed = false
        };
        var token = new ResetPasswordToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(-10),ResetPasswordTokenHash="exampleHash" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ResetPasswordCommand { UserId = "123", ResetPasswordToken = "token", NewPassword = "NewPassword" }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordResetFails()
    {
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string>
            {
                "Client"
            },
            IsEmailConfirmed = false
        };
        var token = new ResetPasswordToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(10) , ResetPasswordTokenHash = "exampleHash" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "NewPassword")).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(new ResetPasswordCommand { UserId = "123", ResetPasswordToken = "token", NewPassword = "NewPassword" }, CancellationToken.None));
    }
}
