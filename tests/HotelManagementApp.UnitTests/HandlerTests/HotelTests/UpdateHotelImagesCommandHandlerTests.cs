using HotelManagementApp.Application.CQRS.HotelOps.UpdateHotelImages;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;

public class UpdateHotelImagesCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IHotelImageRepository> _imageRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly UpdateHotelImagesCommandHandler _handler;

    public UpdateHotelImagesCommandHandlerTests()
    {
        _handler = new UpdateHotelImagesCommandHandler(
            _hotelRepositoryMock.Object,
            _imageRepositoryMock.Object,
            _fileServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowHotelNotFoundException_WhenHotelDoesNotExist()
    {
        var command = new UpdateHotelImagesCommand { HotelId = 1 };

        _hotelRepositoryMock.Setup(m => m.GetHotelById(command.HotelId, default)).ReturnsAsync((Hotel?)null);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _handler.Handle(command, default));
    }


}
