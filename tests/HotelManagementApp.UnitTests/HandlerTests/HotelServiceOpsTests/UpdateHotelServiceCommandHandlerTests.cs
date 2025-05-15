using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Update;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelServiceOpsTests
{
    public class UpdateHotelServiceCommandHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IHotelServiceRepository> _hotelServiceRepositoryMock = new();
        private readonly UpdateHotelServiceCommandHandler _handler;

        public UpdateHotelServiceCommandHandlerTests()
        {
            _handler = new UpdateHotelServiceCommandHandler(
                _hotelRepositoryMock.Object,
                _hotelServiceRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateHotelService_WhenServiceAndHotelExist()
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
                Name = "Old Name",
                Description = "Old Desc",
                Price = 100,
                Hotel = hotel
            };

            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServiceById(1, It.IsAny<CancellationToken>())).ReturnsAsync(service);
            _hotelRepositoryMock.Setup(r => r.GetHotelById(1, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
            _hotelServiceRepositoryMock.Setup(r => r.UpdateHotelService(service, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new UpdateHotelServiceCommand
            {
                Id = 1,
                Name = "New Name",
                Description = "New Description",
                Price = 200,
                HotelId = 1
            };

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("New Name", service.Name);
            Assert.Equal("New Description", service.Description);
            Assert.Equal(200, service.Price);
            Assert.Equal(hotel, service.Hotel);

            _hotelServiceRepositoryMock.Verify(r => r.UpdateHotelService(service, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowHotelServiceNotFoundException_WhenServiceDoesNotExist()
        {
            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServiceById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelService?)null);

            var command = new UpdateHotelServiceCommand
            {
                Id = 2,
                Name = "Name",
                Description = "Description",
                Price = 100,
                HotelId = 1
            };

            await Assert.ThrowsAsync<HotelServiceNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            var service = new HotelService
            {
                Id = 1,
                Name = "Old Name",
                Description = "Old Desc",
                Price = 100,
                Hotel = new Hotel
                {
                    Id = 1,
                    Name = "Test Hotel",
                    Address = "Test Address",
                    City = new City { Id = 1, Name = "Test City", Country = "Test Country", Latitude = 0, Longitude = 0 },
                    PhoneNumber = "123456789",
                    Email = "test@hotel.com",
                    Description = "Test"
                }
            };

            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServiceById(1, It.IsAny<CancellationToken>())).ReturnsAsync(service);
            _hotelRepositoryMock.Setup(r => r.GetHotelById(2, It.IsAny<CancellationToken>())).ReturnsAsync((Hotel?)null);

            var command = new UpdateHotelServiceCommand
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                Price = 100,
                HotelId = 2
            };

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
