using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByHotel;
using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class GetDiscountByHotelQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository = new();
    private readonly Mock<IHotelDiscountRepository> _mockHotelDiscountRepository = new();
    private readonly IRequestHandler<GetDiscountByHotelQuery, ICollection<HotelDiscountResponse>> _handler;

    public GetDiscountByHotelQueryHandlerTests()
    {
        _handler = new GetDiscountByHotelQueryHandler(_mockHotelRepository.Object, _mockHotelDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDiscounts_WhenHotelExists()
    {
        var hotelId = 1;
        var command = new GetDiscountByHotelQuery { HotelId = hotelId };
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

        _mockHotelDiscountRepository
            .Setup(repo => repo.GetDiscountsByTypeId(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<HotelDiscount>
            {
                new HotelDiscount { Id = 1, Hotel = hotel, DiscountPercent = 10, From = DateTime.Now, To = DateTime.Now.AddDays(5) }
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(10, result.First().DiscountPercent);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenHotelDoesNotExist()
    {
        var hotelId = 1;
        var command = new GetDiscountByHotelQuery { HotelId = hotelId };

        _mockHotelRepository
            .Setup(repo => repo.GetHotelById(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
