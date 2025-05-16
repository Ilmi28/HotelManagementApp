using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.AddLoyaltyReward;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class AddLoyaltyRewardCommandHandlerTests
    {
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly AddLoyaltyRewardCommandHandler _handler;

        public AddLoyaltyRewardCommandHandlerTests()
        {
            _handler = new AddLoyaltyRewardCommandHandler(_rewardsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldAddLoyaltyReward_WhenValidRequest()
        {
            var command = new AddLoyaltyRewardCommand
            {
                RewardName = "Test name",
                PointsRequired = 100,
                Description = "Test description"
            };

            _rewardsRepositoryMock
                .Setup(r => r.AddLoyaltyReward(It.IsAny<LoyaltyReward>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _rewardsRepositoryMock.Verify(r => r.AddLoyaltyReward(It.Is<LoyaltyReward>(reward =>
                reward.RewardName == "Test name" &&
                reward.PointsRequired == 100 &&
                reward.Description == "Test description"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                _handler.Handle(null!, CancellationToken.None));
        }
    }
}
