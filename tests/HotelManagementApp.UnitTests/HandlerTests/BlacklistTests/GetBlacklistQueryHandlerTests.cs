using HotelManagementApp.Application.CQRS.Blacklist.GetAll;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Core.Models.GuestModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.BlacklistTests;
public class GetBlacklistQueryHandlerTests
{
    private readonly Mock<IBlacklistRepository> _blacklistRepositoryMock = new();
    private readonly Mock<IUserManager> _userManagerMock = new();
    private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly GetBlacklistQueryHandler _handler;

    public GetBlacklistQueryHandlerTests()
    {
        _handler = new GetBlacklistQueryHandler(
            _blacklistRepositoryMock.Object,
            _userManagerMock.Object,
            _profilePictureRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnBlacklist_WhenValidRequest()
    {
        var command = new GetBlacklistQuery();
        var blacklistedUsers = new List<BlacklistedGuest>
        {
            new BlacklistedGuest { UserId = "123" }
        };
        var user = new UserDto
        {
            Id = "123",
            UserName = "testuser",
            Email = "test@gmail.com",
            Roles = new List<string> { "Client" }
        };
        var profilePicture = new ProfilePicture
        {
            UserId = "123",
            FileName = "profile.jpg"
        };

        _blacklistRepositoryMock.Setup(m => m.GetBlackList(default)).ReturnsAsync(blacklistedUsers);
        _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture("123", default)).ReturnsAsync(profilePicture);
        _fileServiceMock.Setup(c => c.GetFileUrl("images",profilePicture.FileName)).Returns($"http://example.com/images/{profilePicture.FileName}");

        var result = await _handler.Handle(command, default);

        Assert.Single(result);
        Assert.Equal("123", result.First().Id);
        Assert.Equal("http://example.com/images/profile.jpg", result.First().ProfilePicture);
    }
}
