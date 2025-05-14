using HotelManagementApp.Application.CQRS.HotelParkingOps.GetByid;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelParkingOpsTests
{
    public class GetHotelParkingByIdQueryHandlerTests
    {
        private readonly Mock<IHotelParkingRepository> _parkingRepositoryMock = new();
        private readonly Mock<IParkingDiscountService> _discountServiceMock = new();
        private readonly GetHotelParkingByIdQueryHandler _handler;

        public GetHotelParkingByIdQueryHandlerTests()
        {
            _handler = new GetHotelParkingByIdQueryHandler(
                _parkingRepositoryMock.Object,
                _discountServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnHotelParkingResponse_WhenValidRequest()
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
                CarSpaces = 10,
                Description = "Test Parking",
                Price = 100,
                Hotel = hotel
            };

            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parking);
            _discountServiceMock
                .Setup(service => service.CalculateDiscount(parking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            var result = await _handler.Handle(new GetHotelParkingByIdQuery { ParkingId = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(90, result.FinalPrice);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelParkingNotFoundException_WhenParkingDoesNotExist()
        {
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelParking?)null);

            await Assert.ThrowsAsync<HotelParkingNotFoundException>(() =>
                _handler.Handle(new GetHotelParkingByIdQuery { ParkingId = 1 }, CancellationToken.None));
        }
    }
}
