using HotelManagementApp.Application.CQRS.Discount.GetDiscountsByService;
using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.DiscountTests;

public class GetDiscountsByServiceQueryHandlerTests
{
    private readonly Mock<IHotelServiceRepository> _mockServiceRepository = new();
    private readonly Mock<IServiceDiscountRepository> _mockServiceDiscountRepository = new();
    private readonly IRequestHandler<GetDiscountsByServiceQuery, ICollection<ServiceDiscountResponse>> _handler;

    public GetDiscountsByServiceQueryHandlerTests()
    {
        _handler = new GetDiscountsByServiceQueryHandler(_mockServiceRepository.Object, _mockServiceDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDiscounts_WhenServiceExists()
    {
        var serviceId = 1;
        var command = new GetDiscountsByServiceQuery { ServiceId = serviceId };

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
            Id = serviceId,
            Name = "Spa",
            Price = 100,
            Hotel = hotel
        };

        _mockServiceRepository
            .Setup(repo => repo.GetHotelServiceById(serviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(service);

        _mockServiceDiscountRepository
            .Setup(repo => repo.GetDiscountsByTypeId(serviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ServiceDiscount>
            {
                new ServiceDiscount { Id = 1, Service = service, DiscountPercent = 20, From = DateTime.Now, To = DateTime.Now.AddDays(10) }
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(20, result.First().DiscountPercent);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenServiceDoesNotExist()
    {
        var serviceId = 1;
        var command = new GetDiscountsByServiceQuery { ServiceId = serviceId };

        _mockServiceRepository
            .Setup(repo => repo.GetHotelServiceById(serviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelService?)null);

        await Assert.ThrowsAsync<HotelServiceNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
