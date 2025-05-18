using HotelManagementApp.Application.CQRS.HotelOps.Delete;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelOpsTests;
public class DeleteHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly DeleteHotelCommandHandler _handler;

    public DeleteHotelCommandHandlerTests()
    {
        _handler = new DeleteHotelCommandHandler(_hotelRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteHotel_WhenHotelExists()
    {
        var command = new DeleteHotelCommand { HotelId = 1 };
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
            Name = "Test Hotel",
            Address = "123 Test Street",
            City = city,
            PhoneNumber = "123-456-7890",
            Email = "test@example.com",
            Description = "A test hotel description with more than 50 characters."
        };

        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync(hotel);

        await _handler.Handle(command, default);

        _hotelRepositoryMock.Verify(m => m.RemoveHotel(hotel, default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
    {
        var command = new DeleteHotelCommand { HotelId = 1 };

        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync((Hotel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, default));
    }
}
