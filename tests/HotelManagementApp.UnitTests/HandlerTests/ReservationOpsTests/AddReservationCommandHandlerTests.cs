using HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationTests;

public class AddReservationCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly AddReservationCommandHandler _handler;

    public AddReservationCommandHandlerTests()
    {
        _handler = new AddReservationCommandHandler(
            _orderRepoMock.Object,
            _roomRepoMock.Object,
            _reservationRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldAddReservation_WhenValidRequest()
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
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

        var order = new Order
        {
            Id = 1,
            Status = OrderStatusEnum.Pending,
            UserId = "user1"
        };

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        _orderRepoMock.Setup(r => r.GetOrderById(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _reservationRepoMock.Setup(r => r.GetReservationsByRoomId(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation>());
        _reservationRepoMock.Setup(r => r.AddReservation(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .Callback<Reservation, CancellationToken>((res, ct) => res.Id = 123)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, default);

        Assert.Equal(123, result);
        _reservationRepoMock.Verify(r => r.AddReservation(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowRoomNotFoundException_WhenRoomDoesNotExist()
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
        };

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
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

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        _orderRepoMock.Setup(r => r.GetOrderById(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<OrderNotFoundException>(() => _handler.Handle(command, default));
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(2, 1)]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenDatesInvalid(int fromOffset, int toOffset)
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(fromOffset)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(toOffset))
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

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationConflictException_WhenRoomNotAvailable()
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
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

        var order = new Order
        {
            Id = 1,
            Status = OrderStatusEnum.Pending,
            UserId = "user1"
        };

        var existingReservation = new Reservation
        {
            Id = 10,
            From = command.From,
            To = command.To,
            Room = room,
            Order = new Order { Id = 2, Status = OrderStatusEnum.Confirmed, UserId = "user2" }
        };

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        _orderRepoMock.Setup(r => r.GetOrderById(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _reservationRepoMock.Setup(r => r.GetReservationsByRoomId(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Reservation> { existingReservation });

        await Assert.ThrowsAsync<ReservationConflictException>(() => _handler.Handle(command, default));
    }

    [Theory]
    [InlineData(OrderStatusEnum.Completed)]
    [InlineData(OrderStatusEnum.Cancelled)]
    [InlineData(OrderStatusEnum.Confirmed)]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusInvalid(OrderStatusEnum status)
    {
        var command = new AddReservationCommand
        {
            OrderId = 1,
            RoomId = 2,
            From = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            To = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
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

        var order = new Order
        {
            Id = 1,
            Status = status,
            UserId = "user1"
        };

        _roomRepoMock.Setup(r => r.GetRoomById(command.RoomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        _orderRepoMock.Setup(r => r.GetOrderById(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
