using HotelManagementApp.Application.CQRS.Role.Remove;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using Moq;
using Xunit;

public class RemoveFromRoleCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IUserRolesManager> _userRolesManagerMock = new();
    private readonly RemoveFromRoleCommandHandler _handler;

    public RemoveFromRoleCommandHandlerTests()
    {
        _handler = new RemoveFromRoleCommandHandler(_userManagerMock.Object, _userRolesManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveUserFromRole_WhenValidRequest()
    {
        var command = new RemoveFromRoleCommand { UserId = "123", Role = "Staff" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Staff" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, command.Role)).ReturnsAsync(true);
        _userRolesManagerMock.Setup(m => m.RemoveFromRoleAsync(user, command.Role)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _userRolesManagerMock.Verify(m => m.RemoveFromRoleAsync(user, command.Role), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new RemoveFromRoleCommand { UserId = "123", Role = "Staff" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowPolicyForbiddenException_WhenRoleIsGuest()
    {
        var command = new RemoveFromRoleCommand { UserId = "123", Role = "Guest" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);

        await Assert.ThrowsAsync<PolicyForbiddenException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowRoleConflictException_WhenUserNotInRole()
    {
        var command = new RemoveFromRoleCommand { UserId = "123", Role = "Staff" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, command.Role)).ReturnsAsync(false);

        await Assert.ThrowsAsync<RoleConflictException>(() => _handler.Handle(command, default));
    }
}
