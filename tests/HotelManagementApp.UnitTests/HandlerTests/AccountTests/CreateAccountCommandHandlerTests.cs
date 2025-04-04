using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Responses.AuthResponses;
using MediatR;
using HotelManagementApp.Core.Exceptions;

namespace HotelManagementApp.UnitTests.HandlerTests.AccountTests
{
    public class CreateAccountCommandHandlerTests
    {
        private Mock<IUserManager> _mockUserManager; 
        private Mock<IDbLogger<UserDto>> _mockLogger;
        private IRequestHandler<CreateAccountCommand> _handler;

        public CreateAccountCommandHandlerTests()
        {
            _mockUserManager = new Mock<IUserManager>();
            _mockLogger = new Mock<IDbLogger<UserDto>>();
            _handler = new CreateAccountCommandHandler(_mockUserManager.Object,_mockLogger.Object);
        }  

        [Fact]
        public async Task ValidCommand_CreatesAccount()
        {
            var cmd = new CreateAccountCommand
            {
                Email = "test@mail.com",
                UserName = "test",
                Password = "Password123@",
                Roles = new List<string> { "Client" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((UserDto)null);
            _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync((UserDto)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(), "Password123@")).ReturnsAsync(true);
            _mockLogger.Setup(x => x.Log(OperationEnum.Create, It.IsAny<UserDto>())).Returns(Task.CompletedTask);

            await _handler.Handle(cmd, CancellationToken.None);

            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<UserDto>(), "Password123@"),Times.Once);
            _mockLogger.Verify(x => x.Log(OperationEnum.Create, It.IsAny<UserDto>()), Times.Once);
        }
        [Fact]
        public async Task InvalidCommand_ThrowsUnauthorizedException()
        {
            var cmd = new CreateAccountCommand
            {
                Email = "test@mail.com",
                UserName = "test",
                Password = "Password",
                Roles = new List<string> { "Client" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync((UserDto)null);
            _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync((UserDto)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UserDto>(),"Password")).ReturnsAsync(false);

            var func = async () => await _handler.Handle(cmd,CancellationToken.None);

            await Assert.ThrowsAsync<Exception>(func);
        }

        [InlineData("test@mail.com","test1")]
        [InlineData("test1@mail.com","test")]
        [Theory]

        public async Task UserExists_ThrowsUserAlreadyExistsException(string? email, string? username)
        {
            var cmd = new CreateAccountCommand
            {
                Email = email!,
                UserName = username!,
                Password = "Password123@",
                Roles = new List<string> { "Client" }
            };
            var userDto = new UserDto
            {
                Id = "1",
                Email = email!,
                UserName = username!,
                Roles = new List<string> { "Client" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync("test@mail.com")).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.FindByEmailAsync("test1@mail.com")).ReturnsAsync((UserDto?)null);
            _mockUserManager.Setup(x => x.FindByNameAsync("test")).ReturnsAsync(userDto);
            _mockUserManager.Setup(x => x.FindByNameAsync("test1")).ReturnsAsync((UserDto?)null);

            var func = async () => await _handler.Handle(cmd,CancellationToken.None);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(func);
        }

        public async Task NullArg_ThrowsArgumentNullException()
        {
            var func = async () => await _handler.Handle(null!, CancellationToken.None);
            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }

    }
}
