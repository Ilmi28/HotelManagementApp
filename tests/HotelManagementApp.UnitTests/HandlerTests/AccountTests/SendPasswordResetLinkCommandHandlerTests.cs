using HotelManagementApp.Application.CQRS.Account.SendPasswordResetLink;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests;

public class SendPasswordResetLinkCommandHandlerTests
{
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<IResetPasswordTokenRepository> _tokenRepositoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly SendPasswordResetLinkCommandHandler _handler;

    public SendPasswordResetLinkCommandHandlerTests()
    {
        _emailServiceMock = new Mock<IEmailService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _userManagerMock = new Mock<IUserManager>();
        _tokenRepositoryMock = new Mock<IResetPasswordTokenRepository>();
        _configMock = new Mock<IConfiguration>();
        _handler = new SendPasswordResetLinkCommandHandler(
            _emailServiceMock.Object,
            _tokenServiceMock.Object,
            _userManagerMock.Object,
            _tokenRepositoryMock.Object,
            _configMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldSendEmail_WhenUserExists()
    {
        var user = new UserDto
        {
            Id = "123",
            Email = "test@gmail.com",
            UserName = "testuser",
            Roles = new List<string>
            {
                "Client"
            },
            IsEmailConfirmed = false
        };
        var templatePath = "../HotelManagementApp.Application/HtmlTemplates/ResetPassword.html";
        Directory.CreateDirectory(Path.GetDirectoryName(templatePath)!);
        File.WriteAllText(templatePath, "<html><body><a href='{{ResetPasswordLink}}'>Reset Password</a></body></html>");

        _userManagerMock.Setup(x => x.FindByEmailAsync("test@gmail.com")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.Generate512Token()).Returns("token");
        _configMock.Setup(x => x["FrontendUrl"]).Returns("http://localhost");
        _tokenServiceMock.Setup(x => x.GetTokenHash("token")).Returns("tokenHash");
     

        await _handler.Handle(new SendPasswordResetLinkCommand { Email = "test@gmail.com" }, CancellationToken.None);

        _emailServiceMock.Verify(x => x.SendEmailAsync(
            "test@gmail.com",
            "Reset password",
            It.Is<string>(body => body.Contains("http://localhost/account/reset-password")),
            It.IsAny<CancellationToken>()), Times.Once);

        File.Delete(templatePath);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync("test@gmail.com")).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new SendPasswordResetLinkCommand { Email = "test@gmail.com" }, CancellationToken.None));
    }
}