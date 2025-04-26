using HotelManagementApp.Application.CQRS.Blacklist.Add;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.BlacklistTests;
public class AddToBlacklistCommandHandlerTests
{
    private readonly Mock<IBlacklistRepository> _blacklistRepositoryMock = new();
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IUserRolesManager> _userRolesManagerMock = new();
    private readonly AddToBlacklistCommandHandler _handler;

    public AddToBlacklistCommandHandlerTests()
    {
        _handler = new AddToBlacklistCommandHandler(
            _blacklistRepositoryMock.Object,
            _userManagerMock.Object,
            _userRolesManagerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldAddUserToBlacklist_WhenValidRequest()
    {
        var command = new AddToBlacklistCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(true);
        _blacklistRepositoryMock.Setup(m => m.IsUserBlacklisted(command.UserId, default)).ReturnsAsync(false);

        await _handler.Handle(command, default);

        _blacklistRepositoryMock.Verify(m => m.AddUserToBlacklist(command.UserId, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowPolicyForbiddenException_WhenUserIsNotGuest()
    {
        var command = new AddToBlacklistCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(false);

        await Assert.ThrowsAsync<PolicyForbiddenException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowBlackListConflictException_WhenUserAlreadyBlacklisted()
    {
        var command = new AddToBlacklistCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(true);
        _blacklistRepositoryMock.Setup(m => m.IsUserBlacklisted(command.UserId, default)).ReturnsAsync(true);

        await Assert.ThrowsAsync<BlackListConflictException>(() => _handler.Handle(command, default));
    }
}
