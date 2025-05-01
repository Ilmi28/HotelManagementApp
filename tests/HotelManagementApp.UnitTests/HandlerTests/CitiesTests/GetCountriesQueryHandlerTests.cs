using HotelManagementApp.Application.CQRS.Cities.GetCountries;
using HotelManagementApp.Core.Interfaces.Repositories;
using Moq;
using Xunit;

public class GetCountriesQueryHandlerTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly GetCountriesQueryHandler _handler;

    public GetCountriesQueryHandlerTests()
    {
        _handler = new GetCountriesQueryHandler(_cityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCountries_WhenCountriesExist()
    {
        var command = new GetCountriesQuery();
        var countries = new List<string> { "Poland", "Germany", "France" };

        _cityRepositoryMock.Setup(m => m.GetCountries(default)).ReturnsAsync(countries);

        var result = await _handler.Handle(command, default);

        Assert.Equal(3, result.Count);
        Assert.Contains("Poland", result);
        Assert.Contains("Germany", result);
        Assert.Contains("France", result);
    }
}
