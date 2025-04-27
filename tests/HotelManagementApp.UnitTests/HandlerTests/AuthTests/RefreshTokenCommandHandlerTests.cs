using HotelManagementApp.Application.CQRS.Auth.RefreshSession;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.TokenModels;
using Moq;


namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<ITokenService> _mockTokenManager;
    private readonly Mock<IRefreshTokenRepository> _mockTokenRepository;
    private readonly Mock<IUserManager> _mockUserManager;
    private readonly RefreshSessionCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        _mockTokenManager = new Mock<ITokenService>();
        _mockTokenRepository = new Mock<IRefreshTokenRepository>();
        _mockUserManager = new Mock<IUserManager>();
        _handler = new RefreshSessionCommandHandler(_mockTokenManager.Object, _mockTokenRepository.Object,
                                                    _mockUserManager.Object);
    }

    [Fact]
    public async Task ValidCommand_ReturnsIdentityToken()
    {
        var userDto = new UserDto
        {
            Id = "1",
            Email = "test@mail.com",
            UserName = "test",
            Roles = ["Client"]
        };
        var cmd = new RefreshSessionCommand { RefreshToken = "refreshToken" };
        var token = new RefreshToken
        {
            UserId = userDto.Id,
            RefreshTokenHash = "hashedRefreshToken",
            ExpirationDate = DateTime.Now.AddDays(30)
        };

        _mockTokenManager.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns("hashedRefreshToken");
        _mockTokenManager.Setup(x => x.GenerateIdentityToken(userDto)).Returns("identityToken");
        _mockTokenRepository.Setup(x => x.GetToken(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(token);
        _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDto);

        var response = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal("identityToken", response.IdentityToken);
    }

    [InlineData(null)]
    [InlineData("invalidToken")]
    [Theory]
    public async Task InvalidCommand_ThrowsUnauthorizedException(string? token)
    {
        var cmd = new RefreshSessionCommand { RefreshToken = token! };
        _mockTokenManager.Setup(x => x.GetTokenHash(It.IsAny<string>())).Returns((string?)null);

        var func = async () => await _handler.Handle(cmd, CancellationToken.None);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(func);
    }

    [Fact]
    public async Task NullArg_ThrowsArgumentNullException()
    {
        var func = async () => await _handler.Handle(null!, CancellationToken.None);
        await Assert.ThrowsAsync<ArgumentNullException>(func);
    }

}
