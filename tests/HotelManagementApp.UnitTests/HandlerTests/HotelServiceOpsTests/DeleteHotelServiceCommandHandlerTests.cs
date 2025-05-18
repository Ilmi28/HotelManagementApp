using HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelServiceOpsTests
{
    public class DeleteHotelServiceCommandHandlerTests
    {
        private readonly Mock<IHotelServiceRepository> _hotelServiceRepositoryMock = new();
        private readonly DeleteHotelServiceCommandHandler _handler;

        public DeleteHotelServiceCommandHandlerTests()
        {
            _handler = new DeleteHotelServiceCommandHandler(_hotelServiceRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldDeleteHotelService_WhenServiceExists()
        {
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "Test Address",
                City = new City { Id = 1, Name = "Test City", Country = "Test Country", Latitude = 0, Longitude = 0 },
                PhoneNumber = "123456789",
                Email = "test@hotel.com",
                Description = "Test"
            };
            var service = new HotelService
            {
                Id = 1,
                Name = "Test name",
                Description = "Test description",
                Price = 100,
                Hotel = hotel
            };

            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServiceById(1, It.IsAny<CancellationToken>())).ReturnsAsync(service);
            _hotelServiceRepositoryMock.Setup(r => r.DeleteHotelService(service, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new DeleteHotelServiceCommand { HotelServiceId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            _hotelServiceRepositoryMock.Verify(r => r.DeleteHotelService(service, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowHotelServiceNotFoundException_WhenServiceDoesNotExist()
        {
            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServiceById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelService?)null);

            var command = new DeleteHotelServiceCommand { HotelServiceId = 2 };

            await Assert.ThrowsAsync<HotelServiceNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}

