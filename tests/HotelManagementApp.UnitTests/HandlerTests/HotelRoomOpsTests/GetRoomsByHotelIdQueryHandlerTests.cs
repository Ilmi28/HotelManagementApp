using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelRoomOps.GetByHotelId;
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
    public class GetRoomsByHotelIdQueryHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IRoomImageRepository> _roomImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly Mock<IRoomDiscountService> _roomDiscountServiceMock = new();
        private readonly GetRoomsByHotelIdQueryHandler _handler;

        public GetRoomsByHotelIdQueryHandlerTests()
        {
            _handler = new GetRoomsByHotelIdQueryHandler(
                _roomRepositoryMock.Object,
                _hotelRepositoryMock.Object,
                _roomImageRepositoryMock.Object,
                _fileServiceMock.Object,
                _roomDiscountServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnRooms_WhenHotelExists()
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
            var rooms = new List<HotelRoom>
            {
                new HotelRoom
                {
                    Id = 1,
                    Hotel = hotel,
                    RoomName = "Room 1",
                    RoomType = RoomTypeEnum.Economy,
                    Price = 100,
                    Description = "Desc"
                }
            };
            _hotelRepositoryMock.Setup(r => r.GetHotelById(1, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
            _roomRepositoryMock.Setup(r => r.GetRoomsByHotelId(1, It.IsAny<CancellationToken>())).ReturnsAsync(rooms);
            _roomDiscountServiceMock.Setup(r => r.CalculateDiscount(rooms.First(), It.IsAny<CancellationToken>())).ReturnsAsync(10);
            _roomImageRepositoryMock.Setup(r => r.GetRoomImagesByRoomId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelRoomImage>
                {
                    new HotelRoomImage { Id = 1, FileName = "img.jpg", Room = rooms.First() }
                });
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img.jpg")).Returns("http://test/img.jpg");

            var result = await _handler.Handle(new GetRoomsByHotelIdQuery { HotelId = 1 }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Room 1", result.First().RoomName);
            Assert.Equal(10, result.First().DiscountPercent);
            Assert.Equal(90, result.First().FinalPrice);
            Assert.Single(result.First().RoomImages);
            Assert.Equal("http://test/img.jpg", result.First().RoomImages.First());
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            _hotelRepositoryMock.Setup(r => r.GetHotelById(2, It.IsAny<CancellationToken>())).ReturnsAsync((Hotel?)null);

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(new GetRoomsByHotelIdQuery { HotelId = 2 }, CancellationToken.None));
        }
    }
}
