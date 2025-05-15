using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.ExchangePointsForReward;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class ExchangePointsForRewardCommandHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly Mock<ILoyaltyPointsRepository> _loyaltyPointsRepositoryMock = new();
        private readonly Mock<ILoyaltyRewardUserRepository> _rewardUserRepositoryMock = new();
        private readonly ExchangePointsForRewardCommandHandler _handler;

        public ExchangePointsForRewardCommandHandlerTests()
        {
            _handler = new ExchangePointsForRewardCommandHandler(
                _userManagerMock.Object,
                _rewardsRepositoryMock.Object,
                _loyaltyPointsRepositoryMock.Object,
                _rewardUserRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldExchangePointsForReward_WhenUserAndRewardExistAndHasEnoughPoints()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };
            var reward = new LoyaltyReward
            {
                Id = 1,
                RewardName = "Test name",
                PointsRequired = 100,
                Description = "Test description"
            };
            var points = new LoyaltyPoints
            {
                Id = 1,
                GuestId = "123",
                Points = 150
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(1, It.IsAny<CancellationToken>())).ReturnsAsync(reward);
            _loyaltyPointsRepositoryMock.Setup(r => r.GetLoyaltyPointsByGuestId("123", It.IsAny<CancellationToken>())).ReturnsAsync(points);
            _loyaltyPointsRepositoryMock.Setup(r => r.UpdateLoyaltyPoints(points, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _rewardUserRepositoryMock.Setup(r => r.AddLoyaltyRewardUser(It.IsAny<LoyaltyRewardUser>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new ExchangePointsForRewardCommand
            {
                GuestId = "123",
                RewardId = 1
            };

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(50, points.Points);
            _loyaltyPointsRepositoryMock.Verify(r => r.UpdateLoyaltyPoints(points, It.IsAny<CancellationToken>()), Times.Once);
            _rewardUserRepositoryMock.Verify(r => r.AddLoyaltyRewardUser(It.Is<LoyaltyRewardUser>(ru =>
                ru.UserId == "123" && ru.LoyaltyReward == reward
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

            var command = new ExchangePointsForRewardCommand
            {
                GuestId = "123",
                RewardId = 1
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserIsNotGuest()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { }
            };
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);

            var command = new ExchangePointsForRewardCommand
            {
                GuestId = "123",
                RewardId = 1
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowLoyaltyRewardNotFoundException_WhenRewardDoesNotExist()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(2, It.IsAny<CancellationToken>())).ReturnsAsync((LoyaltyReward?)null);

            var command = new ExchangePointsForRewardCommand
            {
                GuestId = "123",
                RewardId = 2
            };

            await Assert.ThrowsAsync<LoyaltyRewardNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowInvalidOperationException_WhenNotEnoughPoints()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };
            var reward = new LoyaltyReward
            {
                Id = 3,
                RewardName = "Test name",
                PointsRequired = 200,
                Description = "Test description"
            };
            var points = new LoyaltyPoints
            {
                Id = 1,
                GuestId = "123",
                Points = 100
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(3, It.IsAny<CancellationToken>())).ReturnsAsync(reward);
            _loyaltyPointsRepositoryMock.Setup(r => r.GetLoyaltyPointsByGuestId("123", It.IsAny<CancellationToken>())).ReturnsAsync(points);

            var command = new ExchangePointsForRewardCommand
            {
                GuestId = "123",
                RewardId = 3
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
