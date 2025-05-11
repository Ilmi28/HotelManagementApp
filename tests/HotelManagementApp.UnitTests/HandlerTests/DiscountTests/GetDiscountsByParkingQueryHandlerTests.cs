using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByParking;
using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class GetDiscountsByParkingQueryHandlerTests
{
    private readonly Mock<IHotelParkingRepository> _mockParkingRepository = new();
    private readonly Mock<IParkingDiscountRepository> _mockParkingDiscountRepository = new();
    private readonly IRequestHandler<GetDiscountsByParkingQuery, ICollection<ParkingDiscountResponse>> _handler;

    public GetDiscountsByParkingQueryHandlerTests()
    {
        _handler = new GetDiscountsByParkingQueryHandler(_mockParkingRepository.Object, _mockParkingDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDiscounts_WhenParkingExists()
    {
        var parkingId = 1;
        var command = new GetDiscountsByParkingQuery { ParkingId = parkingId };
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

        _mockParkingRepository
            .Setup(repo => repo.GetHotelParkingById(parkingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HotelParking { Id = parkingId, CarSpaces = 1, Price = 1, Hotel = hotel });

        _mockParkingDiscountRepository
            .Setup(repo => repo.GetDiscountsByTypeId(parkingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ParkingDiscount>
            {
                new ParkingDiscount { Id = 1, Parking = new HotelParking { Id = parkingId, CarSpaces = 1, Price = 1, Hotel = hotel}, DiscountPercent = 15, From = DateTime.Now, To = DateTime.Now.AddDays(5) }
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(15, result.First().DiscountPercent);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenParkingDoesNotExist()
    {
        var parkingId = 1;
        var command = new GetDiscountsByParkingQuery { ParkingId = parkingId };

        _mockParkingRepository
            .Setup(repo => repo.GetHotelParkingById(parkingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelParking?)null);

        await Assert.ThrowsAsync<HotelParkingNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
