using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryByGuest;
using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetLoyaltyPointsHistoryByGuestIdQueryHandlerTests
    {
        private readonly Mock<ILoyaltyPointsHistoryRepository> _historyRepositoryMock = new();
        private readonly GetLoyaltyPointsHistoryByGuestIdQueryHandler _handler;

        public GetLoyaltyPointsHistoryByGuestIdQueryHandlerTests()
        {
            _handler = new GetLoyaltyPointsHistoryByGuestIdQueryHandler(_historyRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnHistory_WhenHistoryExistsForGuest()
        {
            var logs = new List<LoyaltyPointsLog>
            {
                new LoyaltyPointsLog
                {
                    Id = 1,
                    UserId = "123",
                    Points = 50,
                    Description = "Test description",
                    Date = new DateTime(2025, 1, 1)
                }
            };

            _historyRepositoryMock.Setup(r => r.GetLoyaltyPointsHistoryByGuestId("guest1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(logs);

            var query = new GetLoyaltyPointsHistoryByGuestIdQuery { GuestId = "guest1" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, r => r.GuestId == "123" && r.Points == 50 && r.Description == "Test description" && r.Date == new DateTime(2025, 1, 1));
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoHistoryForGuest()
        {
            _historyRepositoryMock.Setup(r => r.GetLoyaltyPointsHistoryByGuestId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LoyaltyPointsLog>());

            var query = new GetLoyaltyPointsHistoryByGuestIdQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
