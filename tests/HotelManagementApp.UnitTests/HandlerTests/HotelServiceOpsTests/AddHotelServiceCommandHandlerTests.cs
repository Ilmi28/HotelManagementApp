using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelServiceOps.Add;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelServiceOpsTests
{
    public class AddHotelServiceCommandHandlerTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IHotelServiceRepository> _hotelServiceRepositoryMock = new();
        private readonly AddHotelServiceCommandHandler _handler;

        public AddHotelServiceCommandHandlerTests()
        {
            _handler = new AddHotelServiceCommandHandler(
                _hotelRepositoryMock.Object,
                _hotelServiceRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldAddHotelService_WhenHotelExists()
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

            _hotelRepositoryMock.Setup(r => r.GetHotelById(1, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
            _hotelServiceRepositoryMock.Setup(r => r.AddHotelService(It.IsAny<HotelService>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new AddHotelServiceCommand
            {
                Name = "Test name",
                Description = "Test description",
                Price = 150,
                HotelId = 1
            };

            await _handler.Handle(command, CancellationToken.None);

            _hotelServiceRepositoryMock.Verify(r => r.AddHotelService(It.Is<HotelService>(s =>
                s.Name == "Test name" &&
                s.Description == "Test description" &&
                s.Price == 150 &&
                s.Hotel == hotel
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            _hotelRepositoryMock.Setup(r => r.GetHotelById(2, It.IsAny<CancellationToken>())).ReturnsAsync((Hotel?)null);

            var command = new AddHotelServiceCommand
            {
                Name = "Test name",
                Description = "Test description",
                Price = 150,
                HotelId = 2
            };

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle((AddHotelServiceCommand)null!, CancellationToken.None));
        }
    }
}
