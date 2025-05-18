using HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelServiceOpsTests
{
    public class GetHotelServicesByHotelIdQueryHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IHotelServiceRepository> _hotelServiceRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly GetHotelServicesByHotelIdQueryHandler _handler;

        public GetHotelServicesByHotelIdQueryHandlerTests()
        {
            _handler = new GetHotelServicesByHotelIdQueryHandler(
                _hotelRepositoryMock.Object,
                _hotelServiceRepositoryMock.Object,
                _pricingServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnHotelServices_WhenHotelExists()
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
            var services = new List<HotelService>
            {
                new HotelService
                {
                    Id = 1,
                    Name = "Test name",
                    Description = "Test description",
                    Price = 100,
                    Hotel = hotel
                }
            };

            _hotelRepositoryMock.Setup(r => r.GetHotelById(1, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
            _hotelServiceRepositoryMock.Setup(r => r.GetHotelServicesByHotel(1, It.IsAny<CancellationToken>())).ReturnsAsync(services);
            _pricingServiceMock.Setup(d => d.CalculatePriceForService(services.First(), It.IsAny<CancellationToken>())).ReturnsAsync(90);

            var query = new GetHotelServicesByHotelIdQuery { HotelId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            var serviceResponse = result.First();
            Assert.Equal(1, serviceResponse.Id);
            Assert.Equal("Test name", serviceResponse.Name);
            Assert.Equal("Test description", serviceResponse.Description);
            Assert.Equal(100, serviceResponse.Price);
            Assert.Equal(10, serviceResponse.Discount);
            Assert.Equal(90, serviceResponse.FinalPrice);
            Assert.Equal(1, serviceResponse.HotelId);
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            _hotelRepositoryMock.Setup(r => r.GetHotelById(2, It.IsAny<CancellationToken>())).ReturnsAsync((Hotel?)null);

            var query = new GetHotelServicesByHotelIdQuery { HotelId = 2 };

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}

