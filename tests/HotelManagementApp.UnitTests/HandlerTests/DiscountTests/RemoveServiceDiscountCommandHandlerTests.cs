using HotelManagementApp.Application.CQRS.Discount.RemoveServiceDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class RemoveServiceDiscountCommandHandlerTests
{
    private readonly Mock<IServiceDiscountRepository> _mockServiceDiscountRepository = new();
    private readonly IRequestHandler<RemoveServiceDiscountCommand> _handler;

    public RemoveServiceDiscountCommandHandlerTests()
    {
        _handler = new RemoveServiceDiscountCommandHandler(_mockServiceDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDiscount_WhenDiscountExists()
    {
        var discountId = 1;
        var command = new RemoveServiceDiscountCommand { DiscountId = discountId };
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
        var service = new HotelService
        {
            Id = 1,
            Name = "test service",
            Price = 50,
            Hotel = hotel
        };
        var discount = new ServiceDiscount
        {
            Id = discountId,
            Service = service,
            DiscountPercent = 20,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(10)
        };

        _mockServiceDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(discount);

        _mockServiceDiscountRepository
            .Setup(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockServiceDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockServiceDiscountRepository.Verify(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDiscountDoesNotExist()
    {
        var discountId = 1;
        var command = new RemoveServiceDiscountCommand { DiscountId = discountId };

        _mockServiceDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ServiceDiscount?)null);

        await Assert.ThrowsAsync<DiscountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockServiceDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockServiceDiscountRepository.Verify(repo => repo.RemoveDiscount(It.IsAny<ServiceDiscount>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
