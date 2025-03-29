using HotelManagementApp.Application.CQRS.Auth.LoginUser;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Responses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.UnitTests.HandlerTests.AuthTests
{
    public class LoginUserCommandHandlerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private IRequestHandler<LoginUserCommand, LoginRegisterResponse> _handler;
        public LoginUserCommandHandlerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _handler = new LoginUserCommandHandler(_mockAuthService.Object);
        }

        [Fact]
        public async Task ValidCommand_ReturnsTokens()
        {
            var cmd = new LoginUserCommand
            {
                Email = "test@gmail.com",
                Password = "Password123@"
            };
            _mockAuthService.Setup(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(("identityToken", "refreshToken"));

            var response = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("identityToken", response.IdentityToken);
            Assert.Equal("refreshToken", response.RefreshToken);
        }

        [Fact]
        public async Task NullCommand_ThrowsArgumentNullException()
        {
            LoginUserCommand cmd = null!;

            var func = async () => await _handler.Handle(cmd, CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

    }
}
