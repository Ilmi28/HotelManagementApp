using HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;


namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class GetAllRoomsQueryHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IRoomImageRepository> _roomImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly GetAllRoomsQueryHandler _handler;

        public GetAllRoomsQueryHandlerTests()
        {
            _handler = new GetAllRoomsQueryHandler(
                _roomRepositoryMock.Object,
                _roomImageRepositoryMock.Object,
                _fileServiceMock.Object,
                _pricingServiceMock.Object);
        }

        [Fact]

        public async Task ShouldReturnAllRooms_WhenValidRequest()
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
            var rooms = new List<HotelRoom>
            {
                new HotelRoom
                {
                    Id = 1,
                    Hotel = hotel,
                    RoomName = "Room 101",
                    RoomType = RoomTypeEnum.Premium,
                    Price = 200,
                    Description = "A test room description."
                }
            };
            _roomRepositoryMock.Setup(repo => repo.GetAllRooms(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rooms);
            _pricingServiceMock.Setup(repo => repo.CalculatePriceForRoom(rooms.First(),It.IsAny<CancellationToken>())).ReturnsAsync(160);
            _roomImageRepositoryMock.Setup(repo => repo.GetRoomImagesByRoomId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelRoomImage>
                {
                    new HotelRoomImage
                    {
                        Id = 1,
                        FileName = "test.jpg",
                        Room = rooms.First()
                    }
                });
            _fileServiceMock.Setup(repo => repo.GetFileUrl("images", "test.jpg"))
               .Returns("http://example.com/images/test.jpg");

            var response = await _handler.Handle(new GetAllRoomsQuery(), CancellationToken.None);
            Assert.NotNull(response);
            Assert.Single(response);
            Assert.Equal("Room 101", response.First().RoomName);
            Assert.Equal(160, response.First().FinalPrice);
            Assert.Equal(20, response.First().DiscountPercent);
           

        }
    }
}
