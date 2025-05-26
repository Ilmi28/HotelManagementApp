using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationParking;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class RemoveReservationParkingCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IReservationParkingRepository> _reservationParkingRepoMock = new();
    private readonly Mock<IHotelParkingRepository> _hotelParkingRepoMock = new();
    private readonly RemoveReservationParkingCommandHandler _handler;

    public RemoveReservationParkingCommandHandlerTests()
    {
        _handler = new RemoveReservationParkingCommandHandler(
            _reservationRepoMock.Object,
            _reservationParkingRepoMock.Object,
            _hotelParkingRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRemoveReservationParking_WhenValid()
    {
        var command = new RemoveReservationParkingCommand { ReservationId = 1, ParkingId = 2 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var parking = new HotelParking { Id = 2, CarSpaces = 1, Price = 10, Hotel = null! };
        var reservationParking = new ReservationParking { Id = 3, Reservation = null!, HotelParking = parking, Quantity = 1 };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking> { reservationParking },
            ReservationServices = new List<ReservationService>()
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);
        _reservationParkingRepoMock.Setup(r => r.RemoveReservationParking(reservationParking, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _reservationParkingRepoMock.Verify(r => r.RemoveReservationParking(reservationParking, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var command = new RemoveReservationParkingCommand { ReservationId = 1, ParkingId = 2 };
        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelParkingNotFoundException_WhenParkingNotFound()
    {
        var command = new RemoveReservationParkingCommand { ReservationId = 1, ParkingId = 2 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking>(),
            ReservationServices = new List<ReservationService>()
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync((HotelParking?)null);

        await Assert.ThrowsAsync<HotelParkingNotFoundException>(() => _handler.Handle(command, default));
    }

    [Theory]
    [InlineData(OrderStatusEnum.Cancelled)]
    [InlineData(OrderStatusEnum.Confirmed)]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusIsInvalid(OrderStatusEnum status)
    {
        var command = new RemoveReservationParkingCommand { ReservationId = 1, ParkingId = 2 };
        var order = new Order { Id = 1, Status = status, UserId = "u" };
        var parking = new HotelParking { Id = 2, CarSpaces = 1, Price = 10, Hotel = null! };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking>
            {
                new ReservationParking { Id = 3, Reservation = null!, HotelParking = parking, Quantity = 1 }
            },
            ReservationServices = new List<ReservationService>()
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelParkingRepoMock.Setup(r => r.GetHotelParkingById(command.ParkingId, It.IsAny<CancellationToken>())).ReturnsAsync(parking);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
