using HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelParkingOpsTests
{
    public class GetHotelParkingsByHotelIdQueryHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IHotelParkingRepository> _parkingRepositoryMock = new();
        private readonly Mock<IParkingDiscountService> _discountServiceMock = new();
        private readonly GetHotelParkingsByHotelIdQueryHandler _handler;

        public GetHotelParkingsByHotelIdQueryHandlerTests()
        {
            _handler = new GetHotelParkingsByHotelIdQueryHandler(
                _hotelRepositoryMock.Object,
                _parkingRepositoryMock.Object,
                _discountServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnHotelParkingResponses_WhenValidRequest()
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
            var parkings = new List<HotelParking>
            {
                new HotelParking { Id = 1, CarSpaces = 10, Price = 100, Hotel = hotel }
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _parkingRepositoryMock
                .Setup(repo => repo.GetHotelParkingsByHotelId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parkings);
            _discountServiceMock
                .Setup(service => service.CalculateDiscount(It.IsAny<HotelParking>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            var result = await _handler.Handle(new GetHotelParkingsByHotelIdQuery { HotelId = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            Assert.Equal(90, result.First().FinalPrice);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel?)null);

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(new GetHotelParkingsByHotelIdQuery { HotelId = 1 }, CancellationToken.None));
        }
    }
}
