using HotelManagementApp.Application.CQRS.VIP.Add;
using HotelManagementApp.Core.Exceptions.Forbidden;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HotelManagementApp.Core.Dtos;
using System.Diagnostics;
using HotelManagementApp.Infrastructure.Database.Identity;
using HotelManagementApp.Core.Exceptions.Conflict;


namespace HotelManagementApp.UnitTests.HandlerTests.VIPTests

{
    public class AddToVipCommandHandlerTests
    {
        public readonly Mock<IVIPRepository> _vipRepositoryMock = new();
        public readonly Mock<IUserManager> _userManagerMock = new();
        public readonly Mock<IUserRolesManager> _userRolesManagerMock = new();
        public readonly AddToVIPCommandHandler _handler;

        public AddToVipCommandHandlerTests()
        {
            _handler = new AddToVIPCommandHandler(_vipRepositoryMock.Object, _userManagerMock.Object, _userRolesManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddUserToVip_WhenValidRequest()
        {
            var command = new AddToVIPCommand { UserId = "123" };
            var user = new UserDto
            {
                Id = "123",
                Email = "test@gmail.com",
                UserName = "testuser",
                Roles = new List<string> { "Guest" }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(true);
            _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(false);

            await _handler.Handle(command, default);
            _vipRepositoryMock.Verify(m => m.IsUserVIP(command.UserId, default), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
        {
            var command = new AddToVIPCommand { UserId = "123" };
            _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync((UserDto?)null);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrowPolicyForbiddenException_WhenUserIsNotAGuest()
        {
            var command = new AddToVIPCommand { UserId = "123" };
            var user = new UserDto
            {
                Id = "123",
                Email = "test@gmail.com",
                UserName = "testuser",
                Roles = new List<string> { }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(false);

            await Assert.ThrowsAsync<PolicyForbiddenException>(() => _handler.Handle(command, default));
        }
        [Fact]
        public async Task Handle_ShouldThrowVIPConflictException_WhenUserIsAlreadyAVip()
        {
            var command = new AddToVIPCommand { UserId = "123" };
            var user = new UserDto
            {
                Id = "123",
                Email = "test@gmail.com",
                UserName = "testuser",
                Roles = new List<string> { "Guest" }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(command.UserId)).ReturnsAsync(user);
            _userRolesManagerMock.Setup(m => m.IsUserInRoleAsync(command.UserId, "Guest")).ReturnsAsync(true);
            _vipRepositoryMock.Setup(m => m.IsUserVIP(command.UserId, default)).ReturnsAsync(true);

            await Assert.ThrowsAsync<VIPConflictException>(() => _handler.Handle(command, default));
        }
    }
}
