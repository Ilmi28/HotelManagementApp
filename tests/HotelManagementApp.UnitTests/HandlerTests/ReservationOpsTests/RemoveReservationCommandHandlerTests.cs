using HotelManagementApp.Application.CQRS.ReservationOps.RemoveReservation;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class RemoveReservationCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IReservationParkingRepository> _reservationParkingRepoMock = new();
    private readonly Mock<IReservationServiceRepository> _reservationServiceRepoMock = new();
    private readonly RemoveReservationCommandHandler _handler;

    public RemoveReservationCommandHandlerTests()
    {
        _handler = new RemoveReservationCommandHandler(
            _reservationRepoMock.Object,
            _reservationParkingRepoMock.Object,
            _reservationServiceRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRemoveReservationAndRelatedEntities_WhenValid()
    {
        var command = new RemoveReservationCommand { ReservationId = 1 };
        var order = new Order { Id = 1, Status = OrderStatusEnum.Pending, UserId = "u" };
        var reservation = new Reservation
        {
            Id = 1,
            From = DateOnly.MinValue,
            To = DateOnly.MinValue,
            Room = null!,
            Order = order,
            ReservationParkings = new List<ReservationParking>
            {
                new ReservationParking { Id = 2, Reservation = null!, HotelParking = null!, Quantity = 1 }
            },
            ReservationServices = new List<ReservationService>
            {
                new ReservationService { Id = 3, Reservation = null!, HotelService = null!, Quantity = 1 }
            }
        };

        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _reservationRepoMock.Setup(r => r.DeleteReservation(reservation, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _reservationParkingRepoMock.Setup(r => r.RemoveReservationParking(It.IsAny<ReservationParking>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _reservationServiceRepoMock.Setup(r => r.RemoveReservationService(It.IsAny<ReservationService>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _handler.Handle(command, default);

        _reservationRepoMock.Verify(r => r.DeleteReservation(reservation, It.IsAny<CancellationToken>()), Times.Once);
        _reservationParkingRepoMock.Verify(r => r.RemoveReservationParking(It.IsAny<ReservationParking>(), It.IsAny<CancellationToken>()), Times.Once);
        _reservationServiceRepoMock.Verify(r => r.RemoveReservationService(It.IsAny<ReservationService>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var command = new RemoveReservationCommand { ReservationId = 1 };
        _reservationRepoMock.Setup(r => r.GetReservationById(command.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(command, default));
    }

    [Theory]
    [InlineData(OrderStatusEnum.Cancelled)]
    [InlineData(OrderStatusEnum.Confirmed)]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenOrderStatusIsInvalid(OrderStatusEnum status)
    {
        var command = new RemoveReservationCommand { ReservationId = 1 };
        var order = new Order { Id = 1, Status = status, UserId = "u" };
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

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}
