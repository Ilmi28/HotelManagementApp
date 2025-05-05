using HotelManagementApp.Application.CQRS.HotelOps.Add;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

public class AddHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly AddHotelCommandHandler _handler;

    public AddHotelCommandHandlerTests()
    {
        _handler = new AddHotelCommandHandler(_hotelRepositoryMock.Object, _cityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddHotel_WhenCityExists()
    {
        var command = new AddHotelCommand
        {
            Name = "Test Hotel",
            Address = "123 Test Street",
            CityId = 1,
            PhoneNumber = "123-456-7890",
            Email = "test@example.com",
            Description = "A test hotel description with more than 50 characters."
        };
        var city = new City
        {
            Id = 1,
            Name = "Test City",
            Latitude = 50.0,
            Longitude = 20.0,
            Country = "Test Country"
        };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.CityId, default)).ReturnsAsync(city);

        await _handler.Handle(command, default);

        _hotelRepositoryMock.Verify(m => m.AddHotel(It.Is<Hotel>(h =>
            h.Name == command.Name &&
            h.Address == command.Address &&
            h.City == city &&
            h.PhoneNumber == command.PhoneNumber &&
            h.Email == command.Email &&
            h.Description == command.Description
        ), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowCityNotFoundException_WhenCityDoesNotExist()
    {
        var command = new AddHotelCommand
        {
            Name = "Test Hotel",
            Address = "123 Test Street",
            CityId = 1,
            PhoneNumber = "123-456-7890",
            Email = "test@example.com",
            Description = "A test hotel description with more than 50 characters."
        };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.CityId, default)).ReturnsAsync((City?)null);

        await Assert.ThrowsAsync<CityNotFoundException>(() => _handler.Handle(command, default));
    }
}
