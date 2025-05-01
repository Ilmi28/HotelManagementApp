
using HotelManagementApp.Application.CQRS.Cities.Refresh;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Xunit;

public class RefreshCitiesCommandHandlerTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly Mock<ICityService> _cityServiceMock = new();
    private readonly RefreshCitiesCommandHandler _handler;

    public RefreshCitiesCommandHandlerTests()
    {
        _handler = new RefreshCitiesCommandHandler(_cityRepositoryMock.Object, _cityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddNewCity_WhenCityDoesNotExist()
    {
        var command = new RefreshCitiesCommand();
        var city = new City { Id = 1, Name = "Warsaw", Country = "Poland", Latitude = 52.2297, Longitude = 21.0122 };
        var cities = new List<City> { city };
        _cityServiceMock.Setup(m => m.FetchCities(default)).Returns(cities.ToAsyncEnumerable());
        _cityRepositoryMock.Setup(m => m.GetCityById(city.Id, default)).ReturnsAsync((City?)null);

        await _handler.Handle(command, default);

        _cityRepositoryMock.Verify(m => m.AddCity(city, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCity_WhenCityExists()
    {
        var command = new RefreshCitiesCommand();
        var city = new City { Id = 1, Name = "Warsaw", Country = "Poland", Latitude = 52.2297, Longitude = 21.0122 };
        var cities = new List<City> { city };
        var existingCity = new City { Id = 1, Name = "OldName", Country = "Poland", Latitude = 50.0, Longitude = 20.0 };

        _cityServiceMock.Setup(m => m.FetchCities(default)).Returns(cities.ToAsyncEnumerable());
        _cityRepositoryMock.Setup(m => m.GetCityById(city.Id, default)).ReturnsAsync(existingCity);

        await _handler.Handle(command, default);

        _cityRepositoryMock.Verify(m => m.UpdateCity(It.Is<City>(c => c.Name == "Warsaw"), default), Times.Once);
    }
}
