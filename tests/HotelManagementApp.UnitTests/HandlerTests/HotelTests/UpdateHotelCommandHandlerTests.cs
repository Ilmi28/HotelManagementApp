using HotelManagementApp.Application.CQRS.Hotel.Update;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

public class UpdateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly UpdateHotelCommandHandler _handler;

    public UpdateHotelCommandHandlerTests()
    {
        _handler = new UpdateHotelCommandHandler(_hotelRepositoryMock.Object, _cityRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateHotel_WhenValidRequest()
    {
        var command = new UpdateHotelCommand
        {
            HotelId = 1,
            Name = "Updated Hotel",
            Address = "456 Updated Street",
            CityId = 2,
            PhoneNumber = "987-654-3210",
            Email = "updated@example.com",
            Description = "An updated hotel description."
        };
        var city = new City
        {
            Id = 2,
            Name = "Updated City",
            Latitude = 50.0,
            Longitude = 20.0,
            Country = "Updated Country"
        };
        var hotel = new HotelModel
        {
            Id = 1,
            Name = "Old Hotel",
            Address = "123 Old Street",
            City = new City
            {
                Id = 1,
                Name = "Test City",
                Latitude = 50.0,
                Longitude = 20.0,
                Country = "Test Country"
            },
            PhoneNumber = "123-456-7890",
            Email = "old@example.com",
            Description = "An old hotel description."
        };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.CityId, default)).ReturnsAsync(city);
        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync(hotel);

        await _handler.Handle(command, default);

        Assert.Equal("Updated Hotel", hotel.Name);
        Assert.Equal("456 Updated Street", hotel.Address);
        Assert.Equal(city, hotel.City);
        Assert.Equal("987-654-3210", hotel.PhoneNumber);
        Assert.Equal("updated@example.com", hotel.Email);
        Assert.Equal("An updated hotel description.", hotel.Description);
        _hotelRepositoryMock.Verify(m => m.UpdateHotel(hotel, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowCityNotFoundException_WhenCityDoesNotExist()
    {
        var command = new UpdateHotelCommand
        {
            HotelId = 1,
            Name = "Updated Hotel",
            Address = "456 Updated Street",
            CityId = 2,
            PhoneNumber = "987-654-3210",
            Email = "updated@example.com",
            Description = "An updated hotel description."
        };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.CityId, default)).ReturnsAsync((City?)null);

        await Assert.ThrowsAsync<CityNotFoundException>(() => _handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
    {
        var command = new UpdateHotelCommand
        {
            HotelId = 1,
            Name = "Updated Hotel",
            Address = "456 Updated Street",
            CityId = 2,
            PhoneNumber = "987-654-3210",
            Email = "updated@example.com",
            Description = "An updated hotel description."
        };
        var city = new City
        {
            Id = 2,
            Name = "Updated City",
            Latitude = 50.0,
            Longitude = 20.0,
            Country = "Updated Country"
        };

        _cityRepositoryMock.Setup(m => m.GetCityById(command.CityId, default)).ReturnsAsync(city);
        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync((HotelModel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, default));
    }
}
