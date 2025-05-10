using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Account.SendConfirmEmailLink;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class SendConfirmEmailLinkCommandHandlerTests
{
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUserManager> _userManagerMock;
    private readonly Mock<IConfirmEmailTokensRepository> _tokenRepositoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly SendConfirmEmailLinkCommandHandler _handler;

    public SendConfirmEmailLinkCommandHandlerTests()
    {
        _emailServiceMock = new Mock<IEmailService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _userManagerMock = new Mock<IUserManager>();
        _tokenRepositoryMock = new Mock<IConfirmEmailTokensRepository>();
        _configMock = new Mock<IConfiguration>();
        _handler = new SendConfirmEmailLinkCommandHandler(
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
        var templatePath = "../HotelManagementApp.Application/HtmlTemplates/ConfirmEmail.html";
        Directory.CreateDirectory(Path.GetDirectoryName(templatePath)!);
        File.WriteAllText(templatePath, "<html><body><a href='{{ConfirmEmailLink}}'>Confirm Email</a></body></html>");

        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.Generate512Token()).Returns("token");
        _configMock.Setup(x => x["FrontendUrl"]).Returns("http://localhost");
        _tokenServiceMock.Setup(x => x.GetTokenHash("token")).Returns("tokenHash");

        await _handler.Handle(new SendConfirmEmailLinkCommand { UserId = "123" }, CancellationToken.None);

        _emailServiceMock.Verify(x => x.SendEmailAsync(
            "test@gmail.com",
            "Confirm email",
            It.Is<string>(body => body.Contains("{{ConfirmEmailLink}}")),
            It.IsAny<CancellationToken>()), Times.Once);

        File.Delete(templatePath);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(new SendConfirmEmailLinkCommand { UserId = "123" }, CancellationToken.None));
    }
}
