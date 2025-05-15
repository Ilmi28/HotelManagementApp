using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetRewardById;
using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetLoyaltyRewardByIdQueryHandlerTests
    {
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly GetLoyaltyRewardByIdQueryHandler _handler;

        public GetLoyaltyRewardByIdQueryHandlerTests()
        {
            _handler = new GetLoyaltyRewardByIdQueryHandler(_rewardsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnReward_WhenRewardExists()
        {
            var reward = new LoyaltyReward
            {
                Id = 1,
                RewardName = "Test name",
                PointsRequired = 50,
                Description = "Test description"
            };

            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reward);

            var query = new GetLoyaltyRewardByIdQuery { LoyaltyRewardId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.RewardId);
            Assert.Equal("Test name", result.RewardName);
            Assert.Equal("Test description", result.RewardDescription);
            Assert.Equal(50, result.Points);
        }

        [Fact]
        public async Task ShouldThrowLoyaltyRewardNotFoundException_WhenRewardDoesNotExist()
        {
            _rewardsRepositoryMock.Setup(r => r.GetLoyaltyRewardById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LoyaltyReward?)null);

            var query = new GetLoyaltyRewardByIdQuery { LoyaltyRewardId = 2 };

            await Assert.ThrowsAsync<LoyaltyRewardNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
