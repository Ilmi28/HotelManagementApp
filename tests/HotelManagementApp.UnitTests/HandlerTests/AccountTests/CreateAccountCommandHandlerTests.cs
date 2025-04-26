using Moq;
using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IUserManager> _mockUserManager;
    private readonly Mock<IAccountDbLogger> _mockLogger;
    private readonly Mock<IProfilePictureRepository> _mockProfilePictureRepository;
    private readonly IRequestHandler<CreateAccountCommand, string> _handler;

    public CreateAccountCommandHandlerTests()
    {
        _mockUserManager = new Mock<IUserManager>();
        _mockLogger = new Mock<IAccountDbLogger>();
        _mockProfilePictureRepository = new Mock<IProfilePictureRepository>();
        _handler = new CreateAccountCommandHandler(_mockUserManager.Object, _mockLogger.Object, _mockProfilePictureRepository.Object);
    }

    [Fact]
    public async Task ValidCommand_CreatesAccount()
    {
        var cmd = new CreateAccountCommand
        {
            Email = "test@mail.com",
            UserName = "test",
            Password = "Password123@",
            Roles = new List<string> { "Client" }
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((UserDto?)null);
        _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync((UserDto?)null);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), "Password123@")).ReturnsAsync(true);
        _mockLogger.Setup(x => x.Log(AccountOperationEnum.Create, It.IsAny<UserDto>())).Returns(Task.CompletedTask);

        var userId = await _handler.Handle(cmd, CancellationToken.None);

        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<UserDto>(), "Password123@"), Times.Once);
        _mockLogger.Verify(x => x.Log(AccountOperationEnum.Create, It.IsAny<UserDto>()), Times.Once);
        Assert.NotNull(userId);
        Assert.NotEmpty(userId);
    }
    [Fact]
    public async Task InvalidCommand_ThrowsUnauthorizedException()
    {
        var cmd = new CreateAccountCommand
        {
            Email = "test@mail.com",
            UserName = "test",
            Password = "Password",
            Roles = new List<string> { "Client" }
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((UserDto?)null);
        _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync((UserDto?)null);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), "Password")).ReturnsAsync(false);

        var func = async () => await _handler.Handle(cmd, CancellationToken.None);

        await Assert.ThrowsAsync<Exception>(func);
    }

    [InlineData("test@mail.com", "test1")]
    [InlineData("test1@mail.com", "test")]
    [Theory]

    public async Task UserExists_ThrowsUserAlreadyExistsException(string? email, string? username)
    {
        var cmd = new CreateAccountCommand
        {
            Email = email!,
            UserName = username!,
            Password = "Password123@",
            Roles = new List<string> { "Client" }
        };
        var userDto = new UserDto
        {
            Id = "1",
            Email = email!,
            UserName = username!,
            Roles = new List<string> { "Client" }
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(userDto);
        _mockUserManager.Setup(x => x.FindByEmailAsync("test1@mail.com")).ReturnsAsync((UserDto?)null);
        _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync(userDto);
        _mockUserManager.Setup(x => x.FindByNameAsync("test1")).ReturnsAsync((UserDto?)null);

        var func = async () => await _handler.Handle(cmd, CancellationToken.None);

        await Assert.ThrowsAsync<UserExistsException>(func);
    }

    [Fact]
    public async Task NullArg_ThrowsArgumentNullException()
    {
        var func = async () => await _handler.Handle(null!, CancellationToken.None);
        await Assert.ThrowsAsync<ArgumentNullException>(func);
    }
}
