using HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;
using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;
public class UpdateProfilePictureCommandHandlerTests
{
    private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly UpdateProfilePictureCommandHandler _handler;

    public UpdateProfilePictureCommandHandlerTests()
    {
        _handler = new UpdateProfilePictureCommandHandler(
            _fileServiceMock.Object,
            _profilePictureRepositoryMock.Object            
        );
    }

    public async Task Handle_ShouldReturnURL_WhenValidRequest()
    {
        var command = new UpdateProfilePictureCommand
        {
            UserId = "123",
            File = new Mock<IFormFile>().Object
        };
        var prevProfilePicture = new ProfilePicture
        {
            UserId = "123",
            FileName = "oldProfile.jpg"
        };
        _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture(command.UserId, default)).ReturnsAsync(prevProfilePicture);
        _fileServiceMock.Setup(m => m.UploadFile("images", It.IsAny<byte[]>(), ".jpg")).Returns("newProfile.jpg");
        _fileServiceMock.Setup(m => m.GetFileUrl("images", "newProfile.jpg")).Returns("http://example.com/images/newProfile.jpg");

        var result = await _handler.Handle(command, default);

        Assert.False(string.IsNullOrEmpty(result));
    }
}
