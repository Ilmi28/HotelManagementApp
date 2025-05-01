using HotelManagementApp.Application.CQRS.Account.Update;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using Moq;


namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class UpdateAccountCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IAccountDbLogger> _loggerMock = new();
    private readonly UpdateAccountCommandHandler _handler;

    public UpdateAccountCommandHandlerTests()
    {
        _handler = new UpdateAccountCommandHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValidRequest()
    {

        var command = new UpdateAccountCommand
        {
            UserId = "123",
            UserName = "newusername",
            Email = "newemail@example.com"
        };
        var user = new UserDto
        {
            Id = "123",
            UserName = "oldusername",
            Email = "oldemail@example.com",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.FindByNameAsync(command.UserName)).ReturnsAsync((UserDto?)null);
        _userManagerMock.Setup(m => m.FindByEmailAsync(command.Email)).ReturnsAsync((UserDto?)null);
        _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        Assert.Equal(command.UserName, user.UserName);
        Assert.Equal(command.Email, user.Email);
        _userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
        _loggerMock.Verify(l => l.Log(AccountOperationEnum.Update, user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
    {
        var command = new UpdateAccountCommand
        {
            UserId = "123",
            UserName = "newusername",
            Email = "newemail@example.com"
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowUserExistsException_WhenUserNameAlreadyExists()
    {
        var command = new UpdateAccountCommand
        {
            UserId = "123",
            UserName = "existingusername",
            Email = "newemail@example.com"
        };
        var user = new UserDto
        {
            Id = "123",
            UserName = "oldusername",
            Email = "oldemail@example.com",
            Roles = new List<string> { "Client" }
        };
        var existingUser = new UserDto
        {
            Id = "456",
            UserName = "existingusername",
            Email = "otheremail@example.com",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.FindByNameAsync(command.UserName)).ReturnsAsync(existingUser);

        await Assert.ThrowsAsync<UserExistsException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowUserExistsException_WhenEmailAlreadyExists()
    {
        var command = new UpdateAccountCommand
        {
            UserId = "123",
            UserName = "newusername",
            Email = "existingemail@example.com"
        };
        var user = new UserDto
        {
            Id = "123",
            UserName = "oldusername",
            Email = "oldemail@example.com",
            Roles = new List<string> { "Client" }
        };
        var existingUser = new UserDto
        {
            Id = "456",
            UserName = "otherusername",
            Email = "existingemail@example.com",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.FindByEmailAsync(command.Email)).ReturnsAsync(existingUser);

        await Assert.ThrowsAsync<UserExistsException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUpdateFails()
    {
        var command = new UpdateAccountCommand
        {
            UserId = "123",
            UserName = "newusername",
            Email = "newemail@example.com"
        };
        var user = new UserDto
        {
            Id = "123",
            UserName = "oldusername",
            Email = "oldemail@example.com",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.FindByNameAsync(command.UserName)).ReturnsAsync((UserDto?)null);
        _userManagerMock.Setup(m => m.FindByEmailAsync(command.Email)).ReturnsAsync((UserDto?)null);
        _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));
    }
}
