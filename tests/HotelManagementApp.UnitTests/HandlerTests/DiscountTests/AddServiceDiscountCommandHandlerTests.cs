using HotelManagementApp.Application.CQRS.Discount.AddServiceDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class AddServiceDiscountCommandHandlerTests
{
    private readonly Mock<IHotelServiceRepository> _mockServiceRepository = new();
    private readonly Mock<IServiceDiscountRepository> _mockServiceDiscountRepository = new();
    private readonly IRequestHandler<AddServiceDiscountCommand> _handler;

    public AddServiceDiscountCommandHandlerTests()
    { 
        _handler = new AddServiceDiscountCommandHandler(_mockServiceRepository.Object, _mockServiceDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDiscount_WhenServiceExists()
    {
        var serviceId = 1;
        var command = new AddServiceDiscountCommand
        {
            ServiceId = serviceId,
            DiscountPercent = 25,
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
        var hotelService = new HotelService
        {
            Id = serviceId,
            Name = "Test Service",
            Description = "A test service description.",
            Price = 50,
            Hotel = hotel
        };
        _mockServiceRepository
            .Setup(repo => repo.GetHotelServiceById(serviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hotelService);

        await _handler.Handle(command, CancellationToken.None);

        _mockServiceDiscountRepository.Verify(repo => repo.AddDiscount(It.IsAny<ServiceDiscount>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenServiceDoesNotExist()
    {
        var serviceId = 1;
        var command = new AddServiceDiscountCommand
        {
            ServiceId = serviceId,
            DiscountPercent = 25,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(5)
        };

        _mockServiceRepository
            .Setup(repo => repo.GetHotelServiceById(serviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelService?)null);

        await Assert.ThrowsAsync<HotelServiceNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
