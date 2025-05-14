using HotelManagementApp.Application.CQRS.HotelParkingOps.Add;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelParkingOpsTests
{
    public class AddHotelParkingCommandHandlerTests
    {
        private readonly Mock<IHotelParkingRepository> _parkingRepositoryMock = new();
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly AddHotelParkingCommandHandler _handler;

        public AddHotelParkingCommandHandlerTests()
        {
            _handler = new AddHotelParkingCommandHandler(
                _parkingRepositoryMock.Object,
                _hotelRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldAddHotelParking_WhenValidRequest()
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

            var command = new AddHotelParkingCommand
            {
                HotelId = 1,
                CarSpaces = 10,
                Description = "Test Parking",
                Price = 50
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);

            await _handler.Handle(command, CancellationToken.None);

            _parkingRepositoryMock.Verify(repo =>
                repo.AddHotelParking(It.Is<HotelParking>(p =>
                    p.CarSpaces == 10 &&
                    p.Description == "Test Parking" &&
                    p.Price == 50 &&
                    p.Hotel == hotel
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            var command = new AddHotelParkingCommand
            {
                HotelId = 99,
                CarSpaces = 10,
                Description = "Test Parking",
                Price = 50
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel?)null);

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(null!, CancellationToken.None));
        }
    }
}
