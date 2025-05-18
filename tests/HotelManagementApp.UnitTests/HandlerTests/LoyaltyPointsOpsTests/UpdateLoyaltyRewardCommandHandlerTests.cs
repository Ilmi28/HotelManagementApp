using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.UpdateLoyaltyReward;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class UpdateLoyaltyRewardCommandHandlerTests
    {
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly UpdateLoyaltyRewardCommandHandler _handler;

        public UpdateLoyaltyRewardCommandHandlerTests()
        {
            _handler = new UpdateLoyaltyRewardCommandHandler(_rewardsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateLoyaltyReward_WhenRewardExists()
        {
            var reward = new LoyaltyReward
            {
                Id = 1,
                RewardName = "Old Name",
                PointsRequired = 50,
                Description = "Old Description"
            };

            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reward);
            _rewardsRepositoryMock.Setup(r => r.UpdateLoyaltyReward(reward, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateLoyaltyRewardCommand
            {
                LoyaltyRewardId = 1,
                RewardName = "New Name",
                PointsRequired = 100,
                Description = "New Description"
            };

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("New Name", reward.RewardName);
            Assert.Equal(100, reward.PointsRequired);
            Assert.Equal("New Description", reward.Description);

            _rewardsRepositoryMock.Verify(r => r.UpdateLoyaltyReward(reward, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowLoyaltyRewardNotFoundException_WhenRewardDoesNotExist()
        {
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LoyaltyReward?)null);

            var command = new UpdateLoyaltyRewardCommand
            {
                LoyaltyRewardId = 2,
                RewardName = "Test name",
                PointsRequired = 100,
                Description = "Test description"
            };

            await Assert.ThrowsAsync<LoyaltyRewardNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}

