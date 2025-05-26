using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class AddReservationParkingCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IHotelParkingRepository> _hotelParkingRepoMock = new();
    private readonly Mock<IReservationParkingRepository> _reservationParkingRepoMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly AddReservationParkingCommandHandler _handler;

    public AddReservationParkingCommandHandlerTests()
    {
        _handler = new AddReservationParkingCommandHandler(
            _reservationRepoMock.Object,
            _hotelParkingRepoMock.Object,
            _reservationParkingRepoMock.Object,
            _roomRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldAddReservationParking_WhenValidRequest()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var parking = new HotelParking { Id = 2, CarSpaces = 10, Description = "", Price = 20, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _reservationParkingRepoMock.Setup(r => r.AddReservationParking(It.IsAny<ReservationParking>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _reservationParkingRepoMock.Verify(r => r.AddReservationParking(It.IsAny<ReservationParking>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelParkingNotFoundException_WhenParkingNotFound()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync((HotelParking?)null);

        await Assert.ThrowsAsync<HotelParkingNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowRoomNotFoundException_WhenRoomNotFound()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var parking = new HotelParking { Id = 2, CarSpaces = 10, Description = "", Price = 20, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusIsCancelledOrConfirmed()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Cancelled, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var parking = new HotelParking { Id = 2, CarSpaces = 10, Description = "", Price = 20, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenParkingAndRoomFromDifferentHotels()
    {
        var command = new AddReservationParkingCommand { ReservationId = 1, ParkingId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel1 = new Hotel { Id = 1, Name = "Hotel1", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var hotel2 = new Hotel { Id = 2, Name = "Hotel2", Address = "ul. Test2", City = city, PhoneNumber = "456", Email = "b@b.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel1 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var parking = new HotelParking { Id = 2, CarSpaces = 10, Description = "", Price = 20, Hotel = hotel2 };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
