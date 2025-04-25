using HotelManagementApp.Application.CQRS.Account.History;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Models.AccountModels;
using Moq;
using Xunit;

public class GetAccountHistoryQueryHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IAccountDbLogger> _loggerMock = new();
    private readonly GetAccountHistoryQueryHandler _handler;

    public GetAccountHistoryQueryHandlerTests()
    {
        _handler = new GetAccountHistoryQueryHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAccountLogResponses_WhenValidRequest()
    {
        var command = new GetAccountHistoryQuery { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        var logs = new List<AccountLog>
        {
            new AccountLog { UserId = "123", AccountOperation = AccountOperationEnum.Login, Date = DateTime.UtcNow },
            new AccountLog { UserId = "123", AccountOperation = AccountOperationEnum.Update, Date = DateTime.UtcNow.AddDays(-1) }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _loggerMock.Setup(m => m.GetLogs(user)).ReturnsAsync(logs);

        var result = await _handler.Handle(command, default);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Login", result.First().Operation);
        Assert.Equal("Update", result.Last().Operation);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
    {
        var command = new GetAccountHistoryQuery { UserId = "123" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
    }
}
