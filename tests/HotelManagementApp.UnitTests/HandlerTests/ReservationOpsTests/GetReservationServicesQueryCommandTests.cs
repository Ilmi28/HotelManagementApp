using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationServices;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class GetReservationServicesQueryCommandTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IReservationServiceRepository> _reservationServiceRepoMock = new();
    private readonly GetReservationServicesQueryCommand _handler;

    public GetReservationServicesQueryCommandTests()
    {
        _handler = new GetReservationServicesQueryCommand(
            _reservationRepoMock.Object,
            _reservationServiceRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnReservationServiceResponses_WhenReservationExists()
    {
        var query = new GetReservationServicesQuery { ReservationId = 1 };
        var reservation = new Reservation { Id = 1, From = DateOnly.MinValue, To = DateOnly.MinValue, Room = null!, Order = null! };
        var service = new HotelService { Id = 2, Name = "Spa", Price = 10, Hotel = null! };
        var reservationService = new ReservationService { Id = 3, Reservation = reservation, HotelService = service, Quantity = 1 };

        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _reservationServiceRepoMock.Setup(r => r.GetReservationServicesByReservationId(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReservationService> { reservationService });

        var result = await _handler.Handle(query, default);

        Assert.Single(result);
        Assert.Equal(reservation.Id, result.First().ReservationId);
        Assert.Equal(service.Id, result.First().ServiceId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoReservationServices()
    {
        var query = new GetReservationServicesQuery { ReservationId = 1 };
        var reservation = new Reservation { Id = 1, From = DateOnly.MinValue, To = DateOnly.MinValue, Room = null!, Order = null! };

        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _reservationServiceRepoMock.Setup(r => r.GetReservationServicesByReservationId(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReservationService>());

        var result = await _handler.Handle(query, default);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var query = new GetReservationServicesQuery { ReservationId = 1 };
        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(query, default));
    }
}
