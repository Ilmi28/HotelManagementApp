using HotelManagementApp.Application.CQRS.Account.DeleteWithoutPassword;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class DeleteWithoutPasswordCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IAccountDbLogger> _loggerMock = new();
    private readonly DeleteWithoutPasswordCommandHandler _handler;

    public DeleteWithoutPasswordCommandHandlerTests()
    {
        _handler = new DeleteWithoutPasswordCommandHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenValidRequest()
    {
        var command = new DeleteWithoutPasswordCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
        _loggerMock.Verify(l => l.Log(AccountOperationEnum.Delete, user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new DeleteWithoutPasswordCommand { UserId = "123" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDeletionFails()
    {
        var command = new DeleteWithoutPasswordCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));
    }
}
