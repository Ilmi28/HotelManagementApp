
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using Microsoft.Extensions.Configuration;
using Xunit;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Core.Interfaces.Services;

public class GetAccountQueryHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly GetAccountQueryHandler _handler;

    public GetAccountQueryHandlerTests()
    {
        _handler = new GetAccountQueryHandler(
            _userManagerMock.Object,
            _profilePictureRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAccountResponse_WhenValidRequest()
    {
        var command = new GetAccountQuery { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };
        var profilePicture = new ProfilePicture
        {
            UserId = "123",
            FileName = "profile.jpg"
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture(user.Id, default)).ReturnsAsync(profilePicture);
        _fileServiceMock.Setup(c => c.GetFileUrl("images",profilePicture.FileName)).Returns($"http://example.com/images/{profilePicture.FileName}");


        var result = await _handler.Handle(command, default);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Roles, result.Roles);
        Assert.Equal($"http://example.com/images/profile.jpg", result.ProfilePicture);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var command = new GetAccountQuery { UserId = "123" };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowProfilePictureNotFoundException_WhenProfilePictureDoesNotExist()
    {
        var command = new GetAccountQuery { UserId = "123" };
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string> { "Client" }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture(user.Id, default)).ReturnsAsync((ProfilePicture?)null);

        await Assert.ThrowsAsync<ProfilePictureNotFoundException>(() => _handler.Handle(command, default));
    }
}
