using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class DeleteAccountCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IAccountDbLogger> _loggerMock = new();
    private readonly DeleteAccountCommandHandler _handler;

    public DeleteAccountCommandHandlerTests()
    {
        _handler = new DeleteAccountCommandHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenValidRequest()
    {
        var command = new DeleteAccountCommand { UserId = "123", Password = "password" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
        _loggerMock.Verify(l => l.Log(AccountOperationEnum.Delete, user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new DeleteAccountCommand { UserId = "123", Password = "password" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenPasswordIsInvalid()
    {
        var command = new DeleteAccountCommand { UserId = "123", Password = "wrongpassword" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDeletionFails()
    {
        var command = new DeleteAccountCommand { UserId = "123", Password = "password" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));
    }
}
