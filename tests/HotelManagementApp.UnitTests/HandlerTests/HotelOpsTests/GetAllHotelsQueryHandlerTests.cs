using HotelManagementApp.Application.CQRS.HotelOps.GetAll;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelOpsTests;
public class GetAllHotelsQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IHotelImageRepository> _imageRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly GetAllHotelsQueryHandler _handler;

    public GetAllHotelsQueryHandlerTests()
    {
        _handler = new GetAllHotelsQueryHandler(
            _hotelRepositoryMock.Object,
            _imageRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAllHotels_WhenHotelsExist()
    {
        var command = new GetAllHotelsQuery();
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
        var images = new List<HotelImage>
        {
            new HotelImage {Id=1, FileName = "image1.jpg",Hotel=hotel }
        };

        _hotelRepositoryMock.Setup(m => m.GetAllHotels(default)).ReturnsAsync(new List<Hotel> { hotel});
        _imageRepositoryMock.Setup(m => m.GetHotelImagesByHotelId(1, default)).ReturnsAsync(images);
        _fileServiceMock.Setup(m => m.GetFileUrl("images", "image1.jpg")).Returns("http://example.com/image1.jpg");

        var result = await _handler.Handle(command, default);

        Assert.Single(result);
        Assert.Equal("Test Hotel", result.First().Name);
        Assert.Equal("http://example.com/image1.jpg", result.First().Images.First());
    }
}
