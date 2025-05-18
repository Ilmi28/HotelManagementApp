using HotelManagementApp.Application.CQRS.Discount.AddRoomDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class AddRoomDiscountCommandHandlerTests
{
    private readonly Mock<IRoomRepository> _mockRoomRepository = new();
    private readonly Mock<IRoomDiscountRepository> _mockRoomDiscountRepository = new();
    private readonly IRequestHandler<AddRoomDiscountCommand> _handler;

    public AddRoomDiscountCommandHandlerTests()
    {
        _handler = new AddRoomDiscountCommandHandler(_mockRoomRepository.Object, _mockRoomDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDiscount_WhenRoomExists()
    {
        var roomId = 1;
        var command = new AddRoomDiscountCommand
        {
            RoomId = roomId,
            DiscountPercent = 10,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(7)
        };
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
        var room = new HotelRoom
        {
            Id = roomId,
            RoomName = "Test Room",
            RoomType = Core.Enums.RoomTypeEnum.Economy,
            Description = "A test room description.",
            Price = 100,
            Hotel = hotel
        };
        _mockRoomRepository
            .Setup(repo => repo.GetRoomById(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        await _handler.Handle(command, CancellationToken.None);

        _mockRoomDiscountRepository.Verify(repo => repo.AddDiscount(It.IsAny<RoomDiscount>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRoomDoesNotExist()
    {
        var roomId = 1;
        var command = new AddRoomDiscountCommand
        {
            RoomId = roomId,
            DiscountPercent = 10,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(7)
        };

        _mockRoomRepository
            .Setup(repo => repo.GetRoomById(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
