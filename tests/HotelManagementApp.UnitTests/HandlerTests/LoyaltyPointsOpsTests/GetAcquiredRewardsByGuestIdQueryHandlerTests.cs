using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAcquiredRewardsByGuest;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetAcquiredRewardsByGuestIdQueryHandlerTests
    {
        private readonly Mock<ILoyaltyRewardUserRepository> _rewardUserRepositoryMock = new();
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly GetAcquiredRewardsByGuestIdQueryHandler _handler;

        public GetAcquiredRewardsByGuestIdQueryHandlerTests()
        {
            _handler = new GetAcquiredRewardsByGuestIdQueryHandler(
                _rewardUserRepositoryMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAcquiredRewards_WhenUserExists()
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
            var rewardUser = new LoyaltyRewardUser
            {
                Id = 1,
                UserId = "123",
                LoyaltyReward = reward,
                Date = new DateTime(2024, 1, 1)
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _rewardUserRepositoryMock.Setup(r => r.GetLoyaltyRewardsByUserId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LoyaltyRewardUser> { rewardUser });

            var query = new GetAcquiredRewardsByGuestIdQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            var response = Assert.Single(result);
            Assert.Equal("123", response.GuestId);
            Assert.Equal(1, response.RewardId);
            Assert.Equal(new DateTime(2024, 1, 1), response.Date);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

            var query = new GetAcquiredRewardsByGuestIdQuery { GuestId = "123" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
