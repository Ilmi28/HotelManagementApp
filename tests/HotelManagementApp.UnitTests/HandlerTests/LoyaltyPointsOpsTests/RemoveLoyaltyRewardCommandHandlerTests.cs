using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.RemoveLoyaltyReward;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class RemoveLoyaltyRewardCommandHandlerTests
    {
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly RemoveLoyaltyRewardCommandHandler _handler;

        public RemoveLoyaltyRewardCommandHandlerTests()
        {
            _handler = new RemoveLoyaltyRewardCommandHandler(_rewardsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldRemoveLoyaltyReward_WhenRewardExists()
        {
            var reward = new LoyaltyReward
            {
                Id = 1,
                RewardName = "Test name",
                PointsRequired = 100,
                Description = "Test description"
            };

            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reward);
            _rewardsRepositoryMock.Setup(r => r.RemoveLoyaltyReward(reward, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new RemoveLoyaltyRewardCommand { LoyaltyRewardId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            _rewardsRepositoryMock.Verify(r => r.RemoveLoyaltyReward(reward, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowLoyaltyRewardNotFoundException_WhenRewardDoesNotExist()
        {
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LoyaltyReward?)null);

            var command = new RemoveLoyaltyRewardCommand { LoyaltyRewardId = 2 };

            await Assert.ThrowsAsync<LoyaltyRewardNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}

