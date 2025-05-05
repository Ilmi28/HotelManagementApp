using HotelManagementApp.Application.CQRS.Cities.GetByCountry;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.CitiesTests;
public class GetCitiesByCountryQueryHandlerTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly GetCitiesByCountryQueryHandler _handler;

    public GetCitiesByCountryQueryHandlerTests()
    {
        _handler = new GetCitiesByCountryQueryHandler(_cityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCities_WhenCountryExists()
    {
        var command = new GetCitiesByCountryQuery { Country = "Poland" };
        var cities = new List<City>
        {
            new City { Id = 1, Name = "Warsaw", Country = "Poland", Latitude = 52.2297, Longitude = 21.0122 },
            new City { Id = 2, Name = "Krakow", Country = "Poland", Latitude = 50.0647, Longitude = 19.9450 }
        };

        _cityRepositoryMock.Setup(m => m.GetCitiesByCountry(command.Country.Normalize(), default)).ReturnsAsync(cities);

        var result = await _handler.Handle(command, default);

        Assert.Equal(2, result.Count);
        Assert.Equal("Warsaw", result.First().Name);
    }
}
