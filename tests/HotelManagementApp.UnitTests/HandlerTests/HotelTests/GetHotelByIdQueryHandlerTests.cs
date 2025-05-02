using HotelManagementApp.Application.CQRS.Hotel.GetById;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

public class GetHotelByIdQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IHotelImageRepository> _imageRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly GetHotelByIdQueryHandler _handler;

    public GetHotelByIdQueryHandlerTests()
    {
        _handler = new GetHotelByIdQueryHandler(
            _hotelRepositoryMock.Object,
            _imageRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnHotel_WhenHotelExists()
    {
        var command = new GetHotelByIdQuery { HotelId = 1 };
        var city = new City
        {
            Id = 1,
            Name = "Test City",
            Latitude = 50.0,
            Longitude = 20.0,
            Country = "Test Country"
        };
        var hotel = new HotelModel
        {
            Id = 1,
            Name = "Test Hotel",
            Address = "123 Test Street",
            City = city,
            PhoneNumber = "123-456-7890",
            Email = "test@example.com",
            Description = "A test hotel description."
        };
        var images = new List<HotelImage>
        {
            new HotelImage {Id=1, FileName = "image1.jpg",Hotel=hotel }
        };

        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync(hotel);
        _imageRepositoryMock.Setup(m => m.GetHotelImagesByHotelId(hotel.Id, default)).ReturnsAsync(images);
        _fileServiceMock.Setup(m => m.GetFileUrl("images", "image1.jpg")).Returns("http://example.com/image1.jpg");

        var result = await _handler.Handle(command, default);

        Assert.Equal("Test Hotel", result.Name);
        Assert.Equal("http://example.com/image1.jpg", result.Images.First());
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
    {
        var command = new GetHotelByIdQuery { HotelId = 1 };

        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync((HotelModel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, default));
    }
}
