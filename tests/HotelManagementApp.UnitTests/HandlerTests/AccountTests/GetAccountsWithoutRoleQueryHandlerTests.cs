using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Account.GetAccountsWithoutRole;
using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Models.AccountModels;
using Moq;
using Xunit;
namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class GetAccountsWithoutRoleQueryHandlerTests
{
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock;
    private readonly GetAccountsWithoutRoleQueryHandler _handler;

    public GetAccountsWithoutRoleQueryHandlerTests()
    {
        _userManagerMock = new Mock<IUserManager>();
        _fileServiceMock = new Mock<IFileService>();
        _profilePictureRepositoryMock = new Mock<IProfilePictureRepository>();
        _handler = new GetAccountsWithoutRoleQueryHandler(_userManagerMock.Object, _fileServiceMock.Object, _profilePictureRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAccountsWithoutRole()
    {
        var users = new List<UserDto>
        {
            new UserDto { Id = "123", UserName = "User1", Email = "user1@example.com", Roles = new List<string>() }
        };
        var profilePicture = new ProfilePicture { UserId = "123", FileName = "profile1.jpg" };
        _userManagerMock.Setup(x => x.GetUsersWithoutRole()).ReturnsAsync(users);
        _profilePictureRepositoryMock.Setup(x => x.GetProfilePicture("123", It.IsAny<CancellationToken>())).ReturnsAsync(profilePicture);
        _fileServiceMock.Setup(x => x.GetFileUrl("images", "profile1.jpg")).Returns("http://example.com/images/profile1.jpg");

        var result = await _handler.Handle(new GetAccountsWithoutRoleQuery(), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("User1", result.First().UserName);
    }

    [Fact]
    public async Task Handle_ShouldThrowProfilePictureNotFoundException_WhenProfilePictureIsMissing()
    {
        var users = new List<UserDto>
        {
            new UserDto { Id = "123", UserName = "User1", Email = "user1@example.com", Roles = new List<string>() }
        };
        _userManagerMock.Setup(x => x.GetUsersWithoutRole()).ReturnsAsync(users);
        _profilePictureRepositoryMock.Setup(x => x.GetProfilePicture("123", It.IsAny<CancellationToken>())).ReturnsAsync((ProfilePicture?)null);

        await Assert.ThrowsAsync<ProfilePictureNotFoundException>(() =>
            _handler.Handle(new GetAccountsWithoutRoleQuery(), CancellationToken.None));
    }
}
