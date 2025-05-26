using HotelManagementApp.Application.CQRS.ReservationOps.GetAvailableDays;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class GetAvailableDaysQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly GetAvailableDaysQueryHandler _handler;

    public GetAvailableDaysQueryHandlerTests()
    {
        _handler = new GetAvailableDaysQueryHandler(
            _reservationRepoMock.Object,
            _roomRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAllDays_WhenNoReservations()
    {
        var query = new GetAvailableDaysQuery
        {
            RoomId = 1,
            From = new DateOnly(2024, 1, 1),
            To = new DateOnly(2024, 1, 3)
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
            Id = 2,
            RoomName = "101",
            RoomType = RoomTypeEnum.Economy,
            Price = 100,
            Description = "desc",
            Hotel = hotel
        };

        _roomRepoMock.Setup(r => r.GetRoomById(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _reservationRepoMock.Setup(r => r.GetReservationsByRoomId(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Reservation>());

        var result = await _handler.Handle(query, default);

        Assert.Equal(new[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 3) }, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnAvailableDays_WhenSomeDaysBooked()
    {
        var query = new GetAvailableDaysQuery
        {
            RoomId = 1,
            From = new DateOnly(2024, 1, 1),
            To = new DateOnly(2024, 1, 5)
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
            Id = 2,
            RoomName = "101",
            RoomType = RoomTypeEnum.Economy,
            Price = 100,
            Description = "desc",
            Hotel = hotel
        };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Confirmed, UserId = "u" };
        var reservation = new Reservation
        {
            Id = 1,
            From = new DateOnly(2024, 1, 2),
            To = new DateOnly(2024, 1, 3),
            Room = room,
            Order = order
        };

        _roomRepoMock.Setup(r => r.GetRoomById(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _reservationRepoMock.Setup(r => r.GetReservationsByRoomId(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Reservation> { reservation });

        var result = await _handler.Handle(query, default);

        Assert.Equal(new[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 4), new DateOnly(2024, 1, 5) }, result);
    }

    [Fact]
    public async Task Handle_ShouldIgnoreCancelledAndPendingReservations()
    {
        var query = new GetAvailableDaysQuery
        {
            RoomId = 1,
            From = new DateOnly(2024, 1, 1),
            To = new DateOnly(2024, 1, 2)
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
            Id = 2,
            RoomName = "101",
            RoomType = RoomTypeEnum.Economy,
            Price = 100,
            Description = "desc",
            Hotel = hotel
        };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Cancelled, UserId = "u" };
        var reservation = new Reservation
        {
            Id = 1,
            From = new DateOnly(2024, 1, 1),
            To = new DateOnly(2024, 1, 2),
            Room = room,
            Order = order
        };

        _roomRepoMock.Setup(r => r.GetRoomById(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _reservationRepoMock.Setup(r => r.GetReservationsByRoomId(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Reservation> { reservation });

        var result = await _handler.Handle(query, default);

        Assert.Equal(new[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2) }, result);
    }

    [Fact]
    public async Task Handle_ShouldThrowRoomNotFoundException_WhenRoomNotFound()
    {
        var query = new GetAvailableDaysQuery
        {
            RoomId = 1,
            From = new DateOnly(2024, 1, 1),
            To = new DateOnly(2024, 1, 2)
        };

        _roomRepoMock.Setup(r => r.GetRoomById(query.RoomId, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(query, default));
    }
}
