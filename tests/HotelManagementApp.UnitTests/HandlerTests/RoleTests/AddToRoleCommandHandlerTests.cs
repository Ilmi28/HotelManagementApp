using HotelManagementApp.Application.CQRS.Role.Add;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.RoleTests;
public class AddToRoleCommandHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IUserRolesManager> _userRolesManagerMock = new();
    private readonly AddToRoleCommandHandler _handler;

    public AddToRoleCommandHandlerTests()
    {
        _handler = new AddToRoleCommandHandler(_userManagerMock.Object, _userRolesManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddUserToRole_WhenValidRequest()
    {
        var command = new AddToRoleCommand { UserId = "123", Role = "Staff" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.GetUserRolesAsync(command.UserId)).ReturnsAsync(user.Roles);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, command.Role)).ReturnsAsync(false);
        _userRolesManagerMock.Setup(m => m.AddToRoleAsync(user, command.Role)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _userRolesManagerMock.Verify(m => m.AddToRoleAsync(user, command.Role), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new AddToRoleCommand { UserId = "123", Role = "Staff" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowRoleConflictException_WhenUserAlreadyInRole()
    {
        var command = new AddToRoleCommand { UserId = "123", Role = "Staff" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Staff" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.GetUserRolesAsync(command.UserId)).ReturnsAsync(user.Roles);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, command.Role)).ReturnsAsync(true);

        await Assert.ThrowsAsync<RoleConflictException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowRoleForbiddenException_WhenInvalidRoleAssignment()
    {
        var command = new AddToRoleCommand { UserId = "123", Role = "Guest" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Staff" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.GetUserRolesAsync(command.UserId)).ReturnsAsync(user.Roles);

        await Assert.ThrowsAsync<RoleForbiddenException>(() => _handler.Handle(command, default));
    }
}
