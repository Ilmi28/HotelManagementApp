using HotelManagementApp.Application.CQRS.Discount.RemoveHotelDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class RemoveHotelDiscountCommandHandlerTests
{
    private readonly Mock<IHotelDiscountRepository> _mockHotelDiscountRepository = new();
    private readonly IRequestHandler<RemoveHotelDiscountCommand> _handler;

    public RemoveHotelDiscountCommandHandlerTests()
    {
        _handler = new RemoveHotelDiscountCommandHandler(_mockHotelDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDiscount_WhenDiscountExists()
    {
        var discountId = 1;
        var command = new RemoveHotelDiscountCommand { DiscountId = discountId };
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
        var discount = new HotelDiscount
        {
            Id = discountId,
            Hotel = hotel,
            DiscountPercent = 10,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(5)
        };

        _mockHotelDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(discount);

        _mockHotelDiscountRepository
            .Setup(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockHotelDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockHotelDiscountRepository.Verify(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDiscountDoesNotExist()
    {
        var discountId = 1;
        var command = new RemoveHotelDiscountCommand { DiscountId = discountId };

        _mockHotelDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelDiscount?)null);

        await Assert.ThrowsAsync<DiscountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockHotelDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockHotelDiscountRepository.Verify(repo => repo.RemoveDiscount(It.IsAny<HotelDiscount>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
