using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsOfGuests;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetLoyaltyPointsOfGuestsQueryHandlerTests
    {
        private readonly Mock<ILoyaltyPointsRepository> _pointsRepositoryMock = new();
        private readonly GetLoyaltyPointsOfGuestsQueryHandler _handler;

        public GetLoyaltyPointsOfGuestsQueryHandlerTests()
        {
            _handler = new GetLoyaltyPointsOfGuestsQueryHandler(_pointsRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllGuestsPoints()
        {
            var pointsList = new List<LoyaltyPoints>
            {
                new LoyaltyPoints { Id = 1, GuestId = "123", Points = 100 },
            };

            _pointsRepositoryMock.Setup(r => r.GetAllLoyaltyPoints(It.IsAny<CancellationToken>()))
                .ReturnsAsync(pointsList);

            var query = new GetLoyaltyPointsOfGuestsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, r => r.GuestId == "123" && r.Points == 100);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoGuestsPointsExist()
        {
            _pointsRepositoryMock.Setup(r => r.GetAllLoyaltyPoints(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LoyaltyPoints>());

            var query = new GetLoyaltyPointsOfGuestsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
