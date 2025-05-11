using HotelManagementApp.Application.CQRS.Discount.AddHotelDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class AddHotelDiscountCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository = new();
    private readonly Mock<IHotelDiscountRepository> _mockHotelDiscountRepository = new();
    private readonly IRequestHandler<AddHotelDiscountCommand> _handler;

    public AddHotelDiscountCommandHandlerTests()
    {
        _handler = new AddHotelDiscountCommandHandler(_mockHotelRepository.Object, _mockHotelDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDiscount_WhenHotelExists()
    {
        var hotelId = 1;
        var command = new AddHotelDiscountCommand
        {
            HotelId = hotelId,
            DiscountPercent = 15,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(5)
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

        _mockHotelRepository
            .Setup(repo => repo.GetHotelById(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hotel);

        await _handler.Handle(command, CancellationToken.None);

        _mockHotelDiscountRepository.Verify(repo => repo.AddDiscount(It.IsAny<HotelDiscount>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenHotelDoesNotExist()
    {
        var hotelId = 1;
        var command = new AddHotelDiscountCommand
        {
            HotelId = hotelId,
            DiscountPercent = 15,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(5)
        };

        _mockHotelRepository
            .Setup(repo => repo.GetHotelById(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
