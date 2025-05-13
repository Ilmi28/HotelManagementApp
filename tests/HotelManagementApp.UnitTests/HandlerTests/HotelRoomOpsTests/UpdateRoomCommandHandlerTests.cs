using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Update;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class UpdateRoomCommandHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly UpdateRoomCommandHandler _handler;

        public UpdateRoomCommandHandlerTests()
        {
            _handler = new UpdateRoomCommandHandler(_roomRepositoryMock.Object, _hotelRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateRoom_WhenRoomAndHotelExist()
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
            _hotelRepositoryMock.Setup(h => h.GetHotelById(1, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
            _roomRepositoryMock.Setup(r => r.UpdateRoom(It.IsAny<HotelRoom>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new UpdateRoomCommand
            {
                RoomId = 1,
                RoomName = "Updated Room",
                HotelId = 1,
                RoomType = RoomTypeEnum.Premium,
                Price = 200,
                Description = new string('a', 50)
            };

            await _handler.Handle(command, CancellationToken.None);

            _roomRepositoryMock.Verify(r => r.UpdateRoom(It.Is<HotelRoom>(rm =>
                rm.Id == 1 &&
                rm.RoomName == "Updated Room" &&
                rm.RoomType == RoomTypeEnum.Premium &&
                rm.Price == 200 &&
                rm.Description == new string('a', 50) &&
                rm.Hotel == hotel
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowRoomNotFoundException_WhenRoomDoesNotExist()
        {
            _roomRepositoryMock.Setup(r => r.GetRoomById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

            var command = new UpdateRoomCommand
            {
                RoomId = 2,
                RoomName = "Room",
                HotelId = 1,
                RoomType = RoomTypeEnum.Economy,
                Price = 100,
                Description = new string('a', 50)
            };

            await Assert.ThrowsAsync<RoomNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            var room = new HotelRoom
            {
                Id = 1,
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "e", Description = "d" },
                RoomName = "Room",
                RoomType = RoomTypeEnum.Economy,
                Price = 100,
                Description = "Desc"
            };

            _roomRepositoryMock.Setup(r => r.GetRoomById(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
            _hotelRepositoryMock.Setup(h => h.GetHotelById(2, It.IsAny<CancellationToken>())).ReturnsAsync((Hotel?)null);

            var command = new UpdateRoomCommand
            {
                RoomId = 1,
                RoomName = "Room",
                HotelId = 2,
                RoomType = RoomTypeEnum.Economy,
                Price = 100,
                Description = new string('a', 50)
            };

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
