using HotelManagementApp.Application.CQRS.Discount.RemoveParkingDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class RemoveParkingDiscountCommandHandlerTests
{
    private readonly Mock<IParkingDiscountRepository> _mockParkingDiscountRepository = new();
    private readonly IRequestHandler<RemoveParkingDiscountCommand> _handler;

    public RemoveParkingDiscountCommandHandlerTests()
    {
        _handler = new RemoveParkingDiscountCommandHandler(_mockParkingDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDiscount_WhenDiscountExists()
    {
        var discountId = 1;
        var command = new RemoveParkingDiscountCommand { DiscountId = discountId };

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
        var parking = new HotelParking
        {
            Id = 1,
            CarSpaces = 10,
            Description = "Test Parking",
            Price = 20,
            Hotel = hotel
        };
        var discount = new ParkingDiscount
        {
            Id = discountId,
            Parking = parking,
            DiscountPercent = 10,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(5)
        };

        _mockParkingDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(discount);

        _mockParkingDiscountRepository
            .Setup(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockParkingDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockParkingDiscountRepository.Verify(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDiscountDoesNotExist()
    {
        var discountId = 1;
        var command = new RemoveParkingDiscountCommand { DiscountId = discountId };

        _mockParkingDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParkingDiscount?)null);

        await Assert.ThrowsAsync<DiscountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockParkingDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockParkingDiscountRepository.Verify(repo => repo.RemoveDiscount(It.IsAny<ParkingDiscount>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
