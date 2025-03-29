using HotelManagementApp.Application.CQRS.Auth.RefreshToken;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Responses;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests
{
    public class RefreshTokenCommandHandlerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private IRequestHandler<RefreshTokenCommand, RefreshTokenResponse> _handler;

        public RefreshTokenCommandHandlerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _handler = new RefreshTokenCommandHandler(_mockAuthService.Object);
        }

        [Fact]
        public async Task ValidCommand_ReturnsIdentityToken()
        {
            var cmd = new RefreshTokenCommand
            {
                RefreshToken = "refreshToken"
            };

            _mockAuthService.Setup(x => x.RefreshToken(It.IsAny<string>())).ReturnsAsync("identityToken");

            var response = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("identityToken", response.IdentityToken);
        }

        [Fact]
        public async Task NullCommand_ThrowsArgumentNullException()
        {
            RefreshTokenCommand cmd = null!;
            var func = async () => await _handler.Handle(cmd, CancellationToken.None);
            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }
    }
}
