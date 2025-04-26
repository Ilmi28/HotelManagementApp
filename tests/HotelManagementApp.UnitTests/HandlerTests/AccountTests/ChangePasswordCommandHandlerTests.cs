using HotelManagementApp.Application.CQRS.Account.ChangePassword;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IAccountDbLogger> _loggerMock = new();
    private readonly ChangePasswordCommandHandler _handler;

    public ChangePasswordCommandHandlerTests()
    {
        _handler = new ChangePasswordCommandHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldChangePassword_WhenValidRequest()
    {
        var command = new ChangePasswordCommand
        {
            UserId = "123",
            OldPassword = "oldPassword",
            NewPassword = "NewPassword1!"
        };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ChangePasswordAsync(user, command.OldPassword, command.NewPassword)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _userManagerMock.Verify(m => m.ChangePasswordAsync(user, command.OldPassword, command.NewPassword), Times.Once);
        _loggerMock.Verify(l => l.Log(AccountOperationEnum.PasswordChange, user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new ChangePasswordCommand
        {
            UserId = "123",
            OldPassword = "oldPassword",
            NewPassword = "NewPassword1!"
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenOldPasswordIsInvalid()
    {
        var command = new ChangePasswordCommand
        {
            UserId = "123",
            OldPassword = "wrongPassword",
            NewPassword = "NewPassword1!"
        };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ChangePasswordAsync(user, command.OldPassword, command.NewPassword)).ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }
}
