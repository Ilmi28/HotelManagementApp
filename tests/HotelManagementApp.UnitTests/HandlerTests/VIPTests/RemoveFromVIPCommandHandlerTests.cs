using HotelManagementApp.Application.CQRS.VIP.Remove;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using Xunit;

public class RemoveFromVIPCommandHandlerTests
{
    private readonly Mock<IVIPRepository> _vipRepositoryMock = new();
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly RemoveFromVIPCommandHandler _handler;

    public RemoveFromVIPCommandHandlerTests()
    {
        _handler = new RemoveFromVIPCommandHandler(_vipRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveUserFromVIP_WhenUserIsVIP()
    {
        var command = new RemoveFromVIPCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(true);

        await _handler.Handle(command, default);

        _vipRepositoryMock.Verify(m => m.RemoveUserFromVIP(command.UserId, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowVIPNotFoundException_WhenUserIsNotVIP()
    {
        var command = new RemoveFromVIPCommand { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(false);

        await Assert.ThrowsAsync<VIPNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
    {
        var command = new RemoveFromVIPCommand { UserId = "123" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }
}
