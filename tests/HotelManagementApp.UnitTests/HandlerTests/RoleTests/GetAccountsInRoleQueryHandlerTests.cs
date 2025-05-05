using HotelManagementApp.Application.CQRS.Role.GetAll;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using Moq;
using Xunit;

public class GetAccountsInRoleQueryHandlerTests
{
    private readonly Mock<IUserRolesManager> _userRolesManagerMock = new();
    private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly GetAccountsInRoleQueryHandler _handler;

    public GetAccountsInRoleQueryHandlerTests()
    {
        _handler = new GetAccountsInRoleQueryHandler(
            _userRolesManagerMock.Object,
            _profilePictureRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAccountsInRole_WhenValidRequest()
    {
        var command = new GetAccountsInRoleQuery { RoleName = "Staff" };
        var users = new List<UserDto>
        {
            new UserDto { Id = "123", UserName = "testuser", Email = "test@example.com", Roles = new List<string> { "Staff" } }
        };
        var profilePicture = new ProfilePicture { UserId = "123", FileName = "profile.jpg" };

        _userRolesManagerMock.Setup(m => m.GetUsersInRoleAsync(command.RoleName.Normalize())).ReturnsAsync(users);
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture("123", default)).ReturnsAsync(profilePicture);
        _fileServiceMock.Setup(m => m.GetFileUrl("images", "profile.jpg")).Returns("http://example.com/profile.jpg");

        var result = await _handler.Handle(command, default);

        Assert.Single(result);
        Assert.Equal("123", result.First().Id);
        Assert.Equal("http://example.com/profile.jpg", result.First().ProfilePicture);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfilePictureNotFoundException_WhenProfilePictureDoesNotExist()
    {
        var command = new GetAccountsInRoleQuery { RoleName = "Staff" };
        var users = new List<UserDto>
        {
            new UserDto { Id = "123", UserName = "testuser", Email = "test@example.com", Roles = new List<string> { "Staff" } }
        };

        _userRolesManagerMock.Setup(m => m.GetUsersInRoleAsync(command.RoleName.Normalize())).ReturnsAsync(users);
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture("123", default)).ReturnsAsync((ProfilePicture?)null);

        await Assert.ThrowsAsync<ProfilePictureNotFoundException>(() => _handler.Handle(command, default));
    }
}
