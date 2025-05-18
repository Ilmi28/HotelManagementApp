using HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsByGuest;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.LoyaltyPointsOpsTests
{
    public class GetLoyaltyPointsByGuestIdQueryHandlerTests
    {
        private readonly Mock<ILoyaltyPointsRepository> _loyaltyPointsRepositoryMock = new();
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly GetLoyaltyPointsByGuestIdQueryHandler _handler;

        public GetLoyaltyPointsByGuestIdQueryHandlerTests()
        {
            _handler = new GetLoyaltyPointsByGuestIdQueryHandler(
                _loyaltyPointsRepositoryMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task ShouldReturnLoyaltyPoints_WhenUserAndPointsExist()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };
            var points = new LoyaltyPoints
            {
                Id = 1,
                GuestId = "guest1",
                Points = 120
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _loyaltyPointsRepositoryMock.Setup(r => r.GetLoyaltyPointsByGuestId("123", It.IsAny<CancellationToken>())).ReturnsAsync(points);

            var query = new GetLoyaltyPointsByGuestIdQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("123", result.GuestId);
            Assert.Equal(120, result.Points);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

            var query = new GetLoyaltyPointsByGuestIdQuery { GuestId = "123" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowLoyaltyRewardNotFoundException_WhenPointsDoNotExist()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _loyaltyPointsRepositoryMock.Setup(r => r.GetLoyaltyPointsByGuestId("123", It.IsAny<CancellationToken>())).ReturnsAsync((LoyaltyPoints?)null);

            var query = new GetLoyaltyPointsByGuestIdQuery { GuestId = "123" };

            await Assert.ThrowsAsync<LoyaltyRewardNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}

