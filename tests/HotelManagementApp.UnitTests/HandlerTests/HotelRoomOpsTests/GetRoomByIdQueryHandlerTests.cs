using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;
using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class GetRoomByIdQueryHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IRoomImageRepository> _roomImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly Mock<IRoomDiscountService> _roomDiscountServiceMock = new();
        private readonly GetRoomByIdQueryHandler _handler;

        public GetRoomByIdQueryHandlerTests()
        {
            _handler = new GetRoomByIdQueryHandler(
                _roomRepositoryMock.Object,
                _roomImageRepositoryMock.Object,
                _fileServiceMock.Object,
                _roomDiscountServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnRoom_WhenRoomExists()
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
            var room = new HotelRoom
            {
                Id = 1,
                Hotel = hotel,
                RoomName = "Room 1",
                RoomType = RoomTypeEnum.Economy,
                Price = 100,
                Description = "Desc"
            };
            _roomRepositoryMock.Setup(r => r.GetRoomById(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
            _roomDiscountServiceMock.Setup(r => r.CalculateDiscount(room, It.IsAny<CancellationToken>())).ReturnsAsync(15);
            _roomImageRepositoryMock.Setup(r => r.GetRoomImagesByRoomId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelRoomImage>
                {
                    new HotelRoomImage { Id = 1, FileName = "img.jpg", Room = room }
                });
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img.jpg")).Returns("http://test/img.jpg");

            var result = await _handler.Handle(new GetRoomByIdQuery { RoomId = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Room 1", result.RoomName);
            Assert.Equal(15, result.DiscountPercent);
            Assert.Equal(85, result.FinalPrice);
            Assert.Single(result.RoomImages);
            Assert.Equal("http://test/img.jpg", result.RoomImages.First());
        }

        [Fact]
        public async Task ShouldThrowRoomNotFoundException_WhenRoomDoesNotExist()
        {
            _roomRepositoryMock.Setup(r => r.GetRoomById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

            await Assert.ThrowsAsync<RoomNotFoundException>(() =>
                _handler.Handle(new GetRoomByIdQuery { RoomId = 2 }, CancellationToken.None));
        }
    }
}
