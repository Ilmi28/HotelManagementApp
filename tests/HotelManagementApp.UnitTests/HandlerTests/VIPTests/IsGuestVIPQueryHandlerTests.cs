using HotelManagementApp.Application.CQRS.VIP.IsGuestVIP;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using Xunit;

public class IsGuestVIPQueryHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IVIPRepository> _vipRepositoryMock = new();
    private readonly IsGuestVIPQueryHandler _handler;

    public IsGuestVIPQueryHandlerTests()
    {
        _handler = new IsGuestVIPQueryHandler(_userManagerMock.Object, _vipRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenUserIsVIP()
    {
        var command = new IsGuestVIPQuery { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(true);

        var result = await _handler.Handle(command, default);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenUserIsNotVIP()
    {
        var command = new IsGuestVIPQuery { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Guest" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(false);

        var result = await _handler.Handle(command, default);

        Assert.False(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
    {
        var command = new IsGuestVIPQuery { UserId = "123" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }
}
