using HotelManagementApp.Application.CQRS.VIP.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HotelManagementApp.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using HotelManagementApp.Core.Interfaces.Identity;
using Microsoft.Extensions.Configuration;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Services;
using Org.BouncyCastle.Bcpg;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Core.Models.GuestModels;
using HotelManagementApp.Core.Exceptions.NotFound;

namespace HotelManagementApp.UnitTests.HandlerTests.VIPTests
{
    public class GetVIPListQueryHandlerTests
    {
        private readonly Mock<IVIPRepository> _vipRepositoryMock = new();
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IProfilePictureRepository> _profilePictureRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly GetVIPListQueryHandler _handler;

        public GetVIPListQueryHandlerTests()
        {
            _handler = new GetVIPListQueryHandler(
                _vipRepositoryMock.Object,
                _userManagerMock.Object,
                _profilePictureRepositoryMock.Object,
                _fileServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfVips_WhenValidRequest()
        {
            var user = new UserDto
            {
                Id = "123",
                Email = "test@gmail.com",
                UserName = "testuser",
                Roles = new List<string> { "Guest" }
            };

            var vipList = new List<VIPGuest>
            {
                new VIPGuest{UserId = "123" }
            };

            var profilePicture = new ProfilePicture
            {
                UserId ="123",
                FileName = "profile.jpg"
            };

            _vipRepositoryMock.Setup(m => m.GetVIPUsers(default)).ReturnsAsync(vipList);
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture("123", default)).ReturnsAsync(profilePicture);
            _fileServiceMock.Setup(m => m.GetFileUrl("images", "profile.jpg")).Returns("http://example.com/images/profile.jpg");

            var result = await _handler.Handle(new GetVIPListQuery(), default);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("123", result.First().Id);
            Assert.Equal("http://example.com/images/profile.jpg", result.First().ProfilePicture);
        }

        [Fact]
        public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            var vipList = new List<VIPGuest>
            {
                new VIPGuest{UserId = "123" }
            };
            _vipRepositoryMock.Setup(m => m.GetVIPUsers(default)).ReturnsAsync(vipList);
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);
            await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(new GetVIPListQuery(), default));
        }

        [Fact]
        public async Task Handle_ShouldThrowProfilePictureNotFoundException_WhenProfilePictureNotFound()
        {
            var user = new UserDto
            {
                Id = "123",
                Email = "test@gmail.com",
                UserName = "testuser",
                Roles = new List<string> { "Guest" }
            };

            var vipList = new List<VIPGuest>
            {
                new VIPGuest{UserId = "123" }
            };

            _vipRepositoryMock.Setup(m => m.GetVIPUsers(default)).ReturnsAsync(vipList);
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _profilePictureRepositoryMock.Setup(m => m.GetProfilePicture("123", default)).ReturnsAsync((ProfilePicture?)null);

            await Assert.ThrowsAsync<ProfilePictureNotFoundException>(() => _handler.Handle(new GetVIPListQuery(), default));
        }
    }
}
