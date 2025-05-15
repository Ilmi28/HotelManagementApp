using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAvailableRewards;
using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetAvailableRewardsQueryHandlerTests
    {
        private readonly Mock<ILoyaltyRewardsRepository> _rewardsRepositoryMock = new();
        private readonly GetAvailableRewardsQueryHandler _handler;

        public GetAvailableRewardsQueryHandlerTests()
        {
            _handler = new GetAvailableRewardsQueryHandler(_rewardsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllAvailableRewards()
        {
            var rewards = new List<LoyaltyReward>
            {
                new LoyaltyReward
                {
                    Id = 1,
                    RewardName = "Test name1",
                    PointsRequired = 50,
                    Description = "Test description1"
                },
                new LoyaltyReward
                {
                    Id = 2,
                    RewardName = "Test name2",
                    PointsRequired = 30,
                    Description = "Test description2"
                }
            };

            _rewardsRepositoryMock.Setup(r => r.GetAllLoyaltyRewards(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rewards);

            var query = new GetAvailableRewardsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.RewardId == 1 && r.RewardName == "Test name1" && r.Points == 50 && r.RewardDescription == "Test description1");
            Assert.Contains(result, r => r.RewardId == 2 && r.RewardName == "Test name2" && r.Points == 30 && r.RewardDescription == "Test description2");
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoRewardsExist()
        {
            _rewardsRepositoryMock.Setup(r => r.GetAllLoyaltyRewards(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LoyaltyReward>());

            var query = new GetAvailableRewardsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

