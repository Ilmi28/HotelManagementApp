using HotelManagementApp.Application.CQRS.Discount.AddParkingDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class AddParkingDiscountCommandHandlerTests
{
    private readonly Mock<IHotelParkingRepository> _mockParkingRepository = new();
    private readonly Mock<IParkingDiscountRepository> _mockParkingDiscountRepository = new();
    private readonly IRequestHandler<AddParkingDiscountCommand> _handler;

    public AddParkingDiscountCommandHandlerTests()
    { 
        _handler = new AddParkingDiscountCommandHandler(_mockParkingRepository.Object, _mockParkingDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDiscount_WhenParkingExists()
    {
        var parkingId = 1;
        var command = new AddParkingDiscountCommand
        {
            ParkingId = parkingId,
            DiscountPercent = 20,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(10)
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

        _mockParkingRepository
            .Setup(repo => repo.GetHotelParkingById(parkingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HotelParking { Id = parkingId, CarSpaces = 1, Price = 1, Hotel = hotel});

        await _handler.Handle(command, CancellationToken.None);

        _mockParkingDiscountRepository.Verify(repo => repo.AddDiscount(It.IsAny<ParkingDiscount>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenParkingDoesNotExist()
    {
        var parkingId = 1;
        var command = new AddParkingDiscountCommand
        {
            ParkingId = parkingId,
            DiscountPercent = 20,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(10)
        };

        _mockParkingRepository
            .Setup(repo => repo.GetHotelParkingById(parkingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelParking?)null);

        await Assert.ThrowsAsync<HotelParkingNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
