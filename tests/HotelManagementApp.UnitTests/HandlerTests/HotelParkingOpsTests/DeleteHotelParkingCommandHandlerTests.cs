using HotelManagementApp.Application.CQRS.HotelParkingOps.Delete;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelParkingOpsTests
{
    public class DeleteHotelParkingCommandHandlerTests
    {
        private readonly Mock<IHotelParkingRepository> _parkingRepositoryMock = new();
        private readonly DeleteHotelParkingCommandHandler _handler;

        public DeleteHotelParkingCommandHandlerTests()
        {
            _handler = new DeleteHotelParkingCommandHandler(_parkingRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteHotelParking_WhenValidRequest()
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
            var hotelParking = new HotelParking
            {
                Id = 1,
                CarSpaces = 10,
                Description = "Test Parking",
                Price = 50,
                Hotel = hotel
            };
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotelParking);

            await _handler.Handle(new DeleteHotelParkingCommand { Id = 1 }, CancellationToken.None);

            _parkingRepositoryMock.Verify(repo =>
                repo.DeleteHotelParking(hotelParking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelParkingNotFoundException_WhenParkingDoesNotExist()
        {
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelParking?)null);

            await Assert.ThrowsAsync<HotelParkingNotFoundException>(() =>
                _handler.Handle(new DeleteHotelParkingCommand { Id = 1 }, CancellationToken.None));
        }
    }
}
