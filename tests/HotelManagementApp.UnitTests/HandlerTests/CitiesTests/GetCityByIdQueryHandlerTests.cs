using HotelManagementApp.Application.CQRS.Cities.GetById;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

public class GetCityByIdQueryHandlerTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly GetCityByIdQueryHandler _handler;

    public GetCityByIdQueryHandlerTests()
    {
        _handler = new GetCityByIdQueryHandler(_cityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCity_WhenCityExists()
    {
        var command = new GetCityByIdQuery { Id = 1 };
        var city = new City { Id = 1, Name = "Warsaw", Country = "Poland", Latitude = 52.2297, Longitude = 21.0122 };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.Id, default)).ReturnsAsync(city);

        var result = await _handler.Handle(command, default);

        Assert.Equal("Warsaw", result.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowCityNotFoundException_WhenCityDoesNotExist()
    {
        var command = new GetCityByIdQuery { Id = 1 };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.Id, default)).ReturnsAsync((City?)null);

        await Assert.ThrowsAsync<CityNotFoundException>(() => _handler.Handle(command, default));
    }
}
