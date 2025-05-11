using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Account.ConfirmEmail;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class ConfirmEmailCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<IConfirmEmailTokensRepository> _tokenRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _userManagerMock = new Mock<IUserManager>();
        _tokenRepositoryMock = new Mock<IConfirmEmailTokensRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ConfirmEmailCommandHandler(_userManagerMock.Object, _tokenRepositoryMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldConfirmEmail_WhenTokenIsValid()
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
        var token = new ConfirmEmailToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(10), ConfirmEmailTokenHash = "example" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);

        await _handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None);

        Assert.True(user.IsEmailConfirmed);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None));
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
            },
            IsEmailConfirmed = false
        };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns((string?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None));
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
        var token = new ConfirmEmailToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(-10), ConfirmEmailTokenHash = "example" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenEmailAlreadyConfirmed()
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
            IsEmailConfirmed = true
        };
        var token = new ConfirmEmailToken { UserId = "123", ExpirationDate = DateTime.UtcNow.AddMinutes(10), ConfirmEmailTokenHash = "example" };
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedToken");
        _tokenRepositoryMock.Setup(x => x.GetToken("hashedToken", It.IsAny<CancellationToken>())).ReturnsAsync(token);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConfirmEmailCommand { UserId = "123", Token = "token" }, CancellationToken.None));
    }
}
