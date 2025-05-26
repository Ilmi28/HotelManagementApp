using HotelManagementApp.Application.CQRS.ReservationOps.GetReservationParkings;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.ReservationOpsTests;

public class GetReservationParkingsQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<IReservationParkingRepository> _reservationParkingRepoMock = new();
    private readonly GetReservationParkingsQueryHandler _handler;

    public GetReservationParkingsQueryHandlerTests()
    {
        _handler = new GetReservationParkingsQueryHandler(
            _reservationRepoMock.Object,
            _reservationParkingRepoMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnReservationParkingResponses_WhenReservationExists()
    {
        var query = new GetReservationParkingsQuery { ReservationId = 1 };
        var reservation = new Reservation { Id = 1, From = DateOnly.MinValue, To = DateOnly.MinValue, Room = null!, Order = null! };
        var parking = new HotelParking { Id = 2, CarSpaces = 1, Price = 10, Hotel = null! };
        var reservationParking = new ReservationParking { Id = 3, Reservation = reservation, HotelParking = parking, Quantity = 1 };

        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _reservationParkingRepoMock.Setup(r => r.GetReservationParkingsByReservationId(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReservationParking> { reservationParking });

        var result = await _handler.Handle(query, default);

        Assert.Single(result);
        Assert.Equal(reservation.Id, result.First().ReservationId);
        Assert.Equal(parking.Id, result.First().ParkingId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoReservationParkings()
    {
        var query = new GetReservationParkingsQuery { ReservationId = 1 };
        var reservation = new Reservation { Id = 1, From = DateOnly.MinValue, To = DateOnly.MinValue, Room = null!, Order = null! };

        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(reservation);
        _reservationParkingRepoMock.Setup(r => r.GetReservationParkingsByReservationId(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReservationParking>());

        var result = await _handler.Handle(query, default);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowReservationNotFoundException_WhenReservationNotFound()
    {
        var query = new GetReservationParkingsQuery { ReservationId = 1 };
        _reservationRepoMock.Setup(r => r.GetReservationById(query.ReservationId, It.IsAny<CancellationToken>())).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ReservationNotFoundException>(() => _handler.Handle(query, default));
    }
}
