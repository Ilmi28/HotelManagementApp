using HotelManagementApp.Application.CQRS.Blacklist.Remove;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.BlacklistTests;
public class RemoveFromBlacklistCommandHandlerTests
{
    private readonly Mock<IBlacklistRepository> _blacklistRepositoryMock = new();
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly RemoveFromBlacklistCommandHandler _handler;

    public RemoveFromBlacklistCommandHandlerTests()
    {
        _handler = new RemoveFromBlacklistCommandHandler(
            _blacklistRepositoryMock.Object,
            _userManagerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRemoveUserFromBlacklist_WhenValidRequest()
    {
        var command = new RemoveFromBlacklistCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _blacklistRepositoryMock.Setup(m => m.IsUserBlacklisted(command.UserId, default)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _blacklistRepositoryMock.Verify(m => m.RemoveUserFromBlacklist(command.UserId, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBlacklistUserNotFoundException_WhenUserNotBlacklisted()
    {
        var command = new RemoveFromBlacklistCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _blacklistRepositoryMock.Setup(m => m.IsUserBlacklisted(command.UserId, default)).ReturnsAsync(false);

        await Assert.ThrowsAsync<BlacklistUserNotFoundException>(() => _handler.Handle(command, default));
    }
}
