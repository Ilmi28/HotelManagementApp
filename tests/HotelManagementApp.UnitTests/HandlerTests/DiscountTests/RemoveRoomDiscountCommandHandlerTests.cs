using HotelManagementApp.Application.CQRS.Discount.RemoveRoomDiscount;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class RemoveRoomDiscountCommandHandlerTests
{
    private readonly Mock<IRoomDiscountRepository> _mockRoomDiscountRepository = new();
    private readonly IRequestHandler<RemoveRoomDiscountCommand> _handler;

    public RemoveRoomDiscountCommandHandlerTests()
    {
        _handler = new RemoveRoomDiscountCommandHandler(_mockRoomDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDiscount_WhenDiscountExists()
    {
        var discountId = 1;
        var command = new RemoveRoomDiscountCommand { DiscountId = discountId };
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
        var room = new HotelRoom
        {
            Id = 1,
            RoomName = "test room",
            RoomType = Core.Enums.RoomTypeEnum.Economy,
            Description = "test description",
            Price = 200,
            Hotel = hotel
        };

        var discount = new RoomDiscount
        {
            Id = discountId,
            Room = room,
            DiscountPercent = 15,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(7)
        };

        _mockRoomDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(discount);

        _mockRoomDiscountRepository
            .Setup(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockRoomDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRoomDiscountRepository.Verify(repo => repo.RemoveDiscount(discount, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDiscountDoesNotExist()
    {
        var discountId = 1;
        var command = new RemoveRoomDiscountCommand { DiscountId = discountId };

        _mockRoomDiscountRepository
            .Setup(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RoomDiscount?)null);

        await Assert.ThrowsAsync<DiscountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockRoomDiscountRepository.Verify(repo => repo.GetDiscountById(discountId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRoomDiscountRepository.Verify(repo => repo.RemoveDiscount(It.IsAny<RoomDiscount>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
