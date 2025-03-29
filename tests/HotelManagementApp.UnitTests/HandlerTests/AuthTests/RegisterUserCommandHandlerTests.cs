using HotelManagementApp.Application.CQRS.Auth.RegisterUser;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Responses;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests
{
    public class RegisterUserCommandHandlerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private IRequestHandler<RegisterUserCommand, LoginRegisterResponse> _handler;

        public RegisterUserCommandHandlerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _handler = new RegisterUserCommandHandler(_mockAuthService.Object);
        }

        [Fact]
        public async Task ValidCommand_ReturnsTokens()
        {
            var cmd = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@gmail.com",
                Password = "Password123@"
            };

            _mockAuthService.Setup(x => x.RegisterUser(It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<List<string>>())).ReturnsAsync(("identityToken", "refreshToken"));

            var response = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("identityToken", response.IdentityToken);
            Assert.Equal("refreshToken", response.RefreshToken);
        }

        [Fact]
        public async Task NullCommand_ThrowsArgumentNullException()
        {
            RegisterUserCommand cmd = null!;

            var func = async () => await _handler.Handle(cmd, CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }
    }
}
