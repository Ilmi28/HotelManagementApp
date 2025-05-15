using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelServiceOps.GetById;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelServiceOpsTests
{
    public class GetHotelServiceByIdQueryHandlerTests
    {
        private readonly Mock<IHotelServiceRepository> _serviceRepositoryMock = new();
        private readonly Mock<IServiceDiscountService> _discountServiceMock = new();
        private readonly GetHotelServiceByIdQueryHandler _handler;

        public GetHotelServiceByIdQueryHandlerTests()
        {
            _handler = new GetHotelServiceByIdQueryHandler(
                _serviceRepositoryMock.Object,
                _discountServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnHotelService_WhenServiceExists()
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
                Name = "Spa",
                Description = "Desc",
                Price = 200,
                Hotel = hotel
            };

            _serviceRepositoryMock.Setup(r => r.GetHotelServiceById(1, It.IsAny<CancellationToken>())).ReturnsAsync(service);
            _discountServiceMock.Setup(d => d.CalculateDiscount(service, It.IsAny<CancellationToken>())).ReturnsAsync(10);

            var query = new GetHotelServiceByIdQuery { ServiceId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Spa", result.Name);
            Assert.Equal("Desc", result.Description);
            Assert.Equal(200, result.Price);
            Assert.Equal(1, result.HotelId);
            Assert.Equal(10, result.Discount);
            Assert.Equal(180, result.FinalPrice);
        }

        [Fact]
        public async Task ShouldThrowHotelServiceNotFoundException_WhenServiceDoesNotExist()
        {
            _serviceRepositoryMock.Setup(r => r.GetHotelServiceById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelService?)null);

            var query = new GetHotelServiceByIdQuery { ServiceId = 2 };

            await Assert.ThrowsAsync<HotelServiceNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
