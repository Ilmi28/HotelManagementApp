using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.HotelRoomOps.Remove;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class RemoveRoomCommandHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly RemoveRoomCommandHandler _handler;

        public RemoveRoomCommandHandlerTests()
        {
            _handler = new RemoveRoomCommandHandler(_roomRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldRemoveRoom_WhenRoomExists()
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
            _roomRepositoryMock.Setup(r => r.RemoveRoom(room, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new RemoveRoomCommand { RoomId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            _roomRepositoryMock.Verify(r => r.RemoveRoom(room, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowRoomNotFoundException_WhenRoomDoesNotExist()
        {
            _roomRepositoryMock.Setup(r => r.GetRoomById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

            var command = new RemoveRoomCommand { RoomId = 2 };

            await Assert.ThrowsAsync<RoomNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
