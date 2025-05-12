using HotelManagementApp.Application.CQRS.HotelParkingOps.Update;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelParkingTests
{
    public class UpdateHotelParkingCommandHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IHotelParkingRepository> _parkingRepositoryMock = new();
        private readonly UpdateHotelParkingCommandHandler _handler;

        public UpdateHotelParkingCommandHandlerTests()
        {
            _handler = new UpdateHotelParkingCommandHandler(
                _hotelRepositoryMock.Object,
                _parkingRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateHotelParking_WhenValidRequest()
        {
            var city = new City
            {
                Id = 1,
                Name = "Test City",
                Latitude = 50.0,
                Longitude = 20.0,
                Country = "Test Country"
            };

            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "123 Test Street",
                City = city,
                PhoneNumber = "123-456-7890",
                Email = "test@example.com",
                Description = "A test hotel description."
            };
            var parking = new HotelParking
            {
                Id = 1,
                CarSpaces = 5,
                Description = "Old Description",
                Price = 50,
                Hotel = hotel
            };

            var command = new UpdateHotelParkingCommand
            {
                Id = 1,
                HotelId = 1,
                CarSpaces = 10,
                Description = "Updated Description",
                Price = 100
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parking);

            await _handler.Handle(command, CancellationToken.None);

            _parkingRepositoryMock.Verify(repo =>
                repo.UpdateHotelParking(It.Is<HotelParking>(p =>
                    p.Id == 1 &&
                    p.CarSpaces == 10 &&
                    p.Description == "Updated Description" &&
                    p.Price == 100 &&
                    p.Hotel == hotel
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            var command = new UpdateHotelParkingCommand
            {
                Id = 1,
                HotelId = 99,
                CarSpaces = 10,
                Description = "Updated Description",
                Price = 100
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel?)null);

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelParkingNotFoundException_WhenParkingDoesNotExist()
        {
            var city = new City
            {
                Id = 1,
                Name = "Test City",
                Latitude = 50.0,
                Longitude = 20.0,
                Country = "Test Country"
            };

            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "123 Test Street",
                City = city,
                PhoneNumber = "123-456-7890",
                Email = "test@example.com",
                Description = "A test hotel description."
            };

            var command = new UpdateHotelParkingCommand
            {
                Id = 99,
                HotelId = 1,
                CarSpaces = 10,
                Description = "Updated Description",
                Price = 100
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelParking?)null);

            await Assert.ThrowsAsync<HotelParkingNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
