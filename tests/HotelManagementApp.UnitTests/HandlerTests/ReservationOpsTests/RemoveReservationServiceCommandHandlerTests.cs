using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservationService;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class RemoveReservationServiceCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IReservationServiceRepository> _reservationServiceRepoMock = new();
    private readonly Mock<IHotelServiceRepository> _hotelServiceRepoMock = new();
    private readonly RemoveReservationServiceCommandHandler _handler;

    public RemoveReservationServiceCommandHandlerTests()
    {
        _handler = new RemoveReservationServiceCommandHandler(
            _reservationRepoMock.Object,
            _reservationServiceRepoMock.Object,
            _hotelServiceRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRemoveReservationService_WhenValid()
    {
        var command = new RemoveReservationServiceCommand { ReservationId = 1, ServiceId = 2 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 10, Hotel = null! };
        var reservationService = new ReservationService { Id = 3, Reservation = null!, HotelService = service, Quantity = 1 };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking>(),
            ReservationServices = new List<ReservationService> { reservationService }
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);
        _reservationServiceRepoMock.Setup(r => r.RemoveReservationService(reservationService, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _reservationServiceRepoMock.Verify(r => r.RemoveReservationService(reservationService, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var command = new RemoveReservationServiceCommand { ReservationId = 1, ServiceId = 2 };
        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelServiceNotFoundException_WhenServiceNotFound()
    {
        var command = new RemoveReservationServiceCommand { ReservationId = 1, ServiceId = 2 };
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
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync((HotelService?)null);

        await Assert.ThrowsAsync<HotelServiceNotFoundException>(() => _handler.Handle(command, default));
    }

    [Theory]
    [InlineData(OrderStatusEnum.Cancelled)]
    [InlineData(OrderStatusEnum.Confirmed)]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusIsInvalid(OrderStatusEnum status)
    {
        var command = new RemoveReservationServiceCommand { ReservationId = 1, ServiceId = 2 };
        var order = new Order { Id = 1, Status = status, UserId = "u" };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 10, Hotel = null! };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking>(),
            ReservationServices = new List<ReservationService>
            {
                new ReservationService { Id = 3, Reservation = null!, HotelService = service, Quantity = 1 }
            }
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _hotelServiceRepoMock.Setup(r => r.GetHotelServiceById(command.ServiceId, It.IsAny<CancellationToken>())).ReturnsAsync(service);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
