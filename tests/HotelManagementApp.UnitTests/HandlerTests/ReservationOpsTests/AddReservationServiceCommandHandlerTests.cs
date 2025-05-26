using HotelManagementApp.Application.CQRS.ReservationOps.AddReservationService;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class AddReservationServiceCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IHotelServiceRepository> _hotelServiceRepoMock = new();
    private readonly Mock<IReservationServiceRepository> _reservationServiceRepoMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly AddReservationServiceCommandHandler _handler;

    public AddReservationServiceCommandHandlerTests()
    {
        _handler = new AddReservationServiceCommandHandler(
            _reservationRepoMock.Object,
            _hotelServiceRepoMock.Object,
            _reservationServiceRepoMock.Object,
            _roomRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldAddReservationService_WhenValidRequest()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 50, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _reservationServiceRepoMock.Setup(r => r.AddReservationService(It.IsAny<ReservationService>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _reservationServiceRepoMock.Verify(r => r.AddReservationService(It.IsAny<ReservationService>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelServiceNotFoundException_WhenServiceNotFound()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync((HotelService?)null);

        await Assert.ThrowsAsync<HotelServiceNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowRoomNotFoundException_WhenRoomNotFound()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 50, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

        await Assert.ThrowsAsync<RoomNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusIsCancelledOrConfirmed()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel = new Hotel { Id = 1, Name = "Hotel", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Confirmed, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 50, Hotel = hotel };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenServiceAndRoomFromDifferentHotels()
    {
        var command = new AddReservationServiceCommand { ReservationId = 1, ServiceId = 2, Quantity = 1 };
        var city = new City { Id = 1, Name = "City", Latitude = 0, Longitude = 0, Country = "PL" };
        var hotel1 = new Hotel { Id = 1, Name = "Hotel1", Address = "ul. Test", City = city, PhoneNumber = "123", Email = "a@a.pl", Description = "" };
        var hotel2 = new Hotel { Id = 2, Name = "Hotel2", Address = "ul. Test2", City = city, PhoneNumber = "456", Email = "b@b.pl", Description = "" };
        var room = new HotelRoom { Id = 3, RoomName = "101", RoomType = RoomTypeEnum.Economy, Price = 100, Description = "", Hotel = hotel1 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation { Id = 1, From = DateOnly.FromDateTime(DateTime.Today), To = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), Room = room, Order = order };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 50, Hotel = hotel2 };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        _roomRepoMock.Setup(r => r.GetRoomById(reservation.Room.Id, It.IsAny<CancellationToken>())).ReturnsAsync(room);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
