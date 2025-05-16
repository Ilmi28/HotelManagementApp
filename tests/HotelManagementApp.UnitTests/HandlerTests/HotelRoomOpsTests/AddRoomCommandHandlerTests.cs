using HotelManagementApp.Application.CQRS.HotelRoomOps.Add;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class AddRoomCommandHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly AddRoomCommandHandler _handler;

        public AddRoomCommandHandlerTests()
        {
            _handler = new AddRoomCommandHandler(
                _roomRepositoryMock.Object,
                _hotelRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldAddRoom_WhenValidRequest()
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

            var command = new AddRoomCommand
            {
                HotelId = 1,
                RoomName = "Room 101",
                RoomType = RoomTypeEnum.Premium,
                Price = 200,
                Description = "A test room description."
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);

            await _handler.Handle(command, CancellationToken.None);

            _roomRepositoryMock.Verify(repo =>
                repo.AddRoom(It.Is<HotelRoom>(r =>
                    r.RoomName == "Room 101" &&
                    r.RoomType == RoomTypeEnum.Premium &&
                    r.Price == 200 &&
                    r.Description == command.Description &&
                    r.Hotel == hotel
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
        {
            var command = new AddRoomCommand
            {
                HotelId = 99,
                RoomName = "Room 101",
                RoomType = RoomTypeEnum.Premium,
                Price = 200,
                Description = new string('a', 50)
            };

            _hotelRepositoryMock
                .Setup(repo => repo.GetHotelById(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel?)null);

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
