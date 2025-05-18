using HotelManagementApp.Application.CQRS.HotelRoomOps.UpdateRoomImages;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.HotelRoomOpsTests
{
    public class UpdateRoomImagesCommandHandlerTests
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
        private readonly Mock<IRoomImageRepository> _roomImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly UpdateRoomImagesCommandHandler _handler;

        public UpdateRoomImagesCommandHandlerTests()
        {
            _handler = new UpdateRoomImagesCommandHandler(
                _roomRepositoryMock.Object,
                _roomImageRepositoryMock.Object,
                _fileServiceMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateRoomImages_WhenRoomExists()
        {
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "Test Address",
                City = new City { Id = 1, Name = "Test City", Country = "Test Country", Latitude = 0, Longitude = 0 },
                PhoneNumber = "123456789",
                Email = "test@hotel.com",
                Description = "Test"
            };
            var room = new HotelRoom
            {
                Id = 1,
                Hotel = hotel,
                RoomName = "Room 1",
                RoomType = RoomTypeEnum.Economy,
                Price = 100,
                Description = "Desc"
            };
            var oldImages = new List<HotelRoomImage>
            {
                new HotelRoomImage { Id = 1, FileName = "old1.jpg", Room = room },
                new HotelRoomImage { Id = 2, FileName = "old2.jpg", Room = room }
            };

            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.FileName).Returns("new.jpg");
            formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((stream, token) =>
                {
                    var bytes = new byte[] { 1, 2, 3 };
                    stream.Write(bytes, 0, bytes.Length);
                    return Task.CompletedTask;
                });

            _roomRepositoryMock.Setup(r => r.GetRoomById(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
            _roomImageRepositoryMock.Setup(r => r.GetRoomImagesByRoomId(1, It.IsAny<CancellationToken>())).ReturnsAsync(oldImages);
            _fileServiceMock.Setup(f => f.DeleteFile("images", It.IsAny<string>()));
            _fileServiceMock.Setup(f => f.UploadFile("images", It.IsAny<byte[]>(), ".jpg")).Returns("uploaded.jpg");
            _roomImageRepositoryMock.Setup(r => r.AddRoomImage(It.IsAny<HotelRoomImage>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new UpdateRoomImagesCommand
            {
                RoomId = 1,
                RoomImages = new List<IFormFile> { formFileMock.Object }
            };

            await _handler.Handle(command, CancellationToken.None);

            _fileServiceMock.Verify(f => f.DeleteFile("images", "old1.jpg"), Times.Once);
            _fileServiceMock.Verify(f => f.DeleteFile("images", "old2.jpg"), Times.Once);
            _fileServiceMock.Verify(f => f.UploadFile("images", It.IsAny<byte[]>(), ".jpg"), Times.Once);
            _roomImageRepositoryMock.Verify(r => r.AddRoomImage(It.Is<HotelRoomImage>(img =>
                img.FileName == "uploaded.jpg" && img.Room == room
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowRoomNotFoundException_WhenRoomDoesNotExist()
        {
            _roomRepositoryMock.Setup(r => r.GetRoomById(2, It.IsAny<CancellationToken>())).ReturnsAsync((HotelRoom?)null);

            var command = new UpdateRoomImagesCommand
            {
                RoomId = 2,
                RoomImages = new List<IFormFile>()
            };

            await Assert.ThrowsAsync<RoomNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
