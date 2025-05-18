using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByRoom;
using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class GetDiscountsByRoomQueryHandlerTests
{
    private readonly Mock<IRoomRepository> _mockRoomRepository = new();
    private readonly Mock<IRoomDiscountRepository> _mockRoomDiscountRepository = new();
    private readonly IRequestHandler<GetDiscountsByRoomQuery, ICollection<RoomDiscountResponse>> _handler;

    public GetDiscountsByRoomQueryHandlerTests()
    {
        _handler = new GetDiscountsByRoomQueryHandler(_mockRoomRepository.Object, _mockRoomDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDiscounts_WhenRoomExists()
    {
        var roomId = 1;
        var command = new GetDiscountsByRoomQuery { RoomId = roomId };

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
            RoomName = "Deluxe Room",
            RoomType = RoomTypeEnum.Economy,
            Price = 200,
            Description = "A deluxe room",
            Hotel = hotel
        };

        _mockRoomRepository
            .Setup(repo => repo.GetRoomById(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        _mockRoomDiscountRepository
            .Setup(repo => repo.GetDiscountsByTypeId(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RoomDiscount>
            {
                new RoomDiscount { Id = 1, Room = room, DiscountPercent = 15, From = DateTime.Now, To = DateTime.Now.AddDays(7) }
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(15, result.First().DiscountPercent);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRoomDoesNotExist()
    {
        var roomId = 1;
        var command = new GetDiscountsByRoomQuery { RoomId = roomId };

        _mockRoomRepository
            .Setup(repo => repo.GetRoomById(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
