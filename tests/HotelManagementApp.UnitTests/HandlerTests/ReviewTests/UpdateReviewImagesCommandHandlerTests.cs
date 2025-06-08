using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.UpdateReviewImages;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewTests
{
    public class UpdateReviewImagesCommandHandlerTests
    {
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly Mock<IHotelReviewImageRepository> _imageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly UpdateReviewImagesCommandHandler _handler;

        public UpdateReviewImagesCommandHandlerTests()
        {
            _handler = new UpdateReviewImagesCommandHandler(
                _reviewRepositoryMock.Object,
                _imageRepositoryMock.Object,
                _fileServiceMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateReviewImages_WhenReviewExists()
        {
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "a@a", Description = "d" },
                Created = DateTime.Now.AddDays(-2),
                LastModified = DateTime.Now.AddDays(-1),
                Review = "Test",
                Rating = 5
            };
            var oldImages = new List<HotelReviewImage>
            {
                new HotelReviewImage { Id = 1, FileName = "old1.jpg", HotelReview = review },
                new HotelReviewImage { Id = 2, FileName = "old2.jpg", HotelReview = review }
            };

            var fileMock = new Mock<IFormFile>();
            var fileContent = new byte[] { 1, 2, 3 };
            var ms = new MemoryStream(fileContent);
            fileMock.Setup(f => f.FileName).Returns("new1.jpg");
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((stream, token) =>
                {
                    ms.Position = 0;
                    return ms.CopyToAsync(stream, token);
                });

            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _imageRepositoryMock.Setup(r => r.GetReviewImagesByReviewId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(oldImages);
            _fileServiceMock.Setup(f => f.DeleteFile("images", "old1.jpg"));
            _fileServiceMock.Setup(f => f.DeleteFile("images", "old2.jpg"));
            _fileServiceMock.Setup(f => f.UploadFile("images", It.IsAny<byte[]>(), ".jpg"))
                .Returns("uploaded1.jpg");
            _imageRepositoryMock.Setup(r => r.AddReviewImage(It.IsAny<HotelReviewImage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _reviewRepositoryMock.Setup(r => r.UpdateReview(review, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateReviewImagesCommand
            {
                ReviewId = 1,
                ReviewImages = new List<IFormFile> { fileMock.Object }
            };

            await _handler.Handle(command, CancellationToken.None);

            _fileServiceMock.Verify(f => f.DeleteFile("images", "old1.jpg"), Times.Once);
            _fileServiceMock.Verify(f => f.DeleteFile("images", "old2.jpg"), Times.Once);
            _fileServiceMock.Verify(f => f.UploadFile("images", It.IsAny<byte[]>(), ".jpg"), Times.Once);
            _imageRepositoryMock.Verify(r => r.AddReviewImage(It.Is<HotelReviewImage>(img =>
                img.FileName == "uploaded1.jpg" && img.HotelReview == review
            ), It.IsAny<CancellationToken>()), Times.Once);
            _imageRepositoryMock.Verify(r => r.RemoveReviewImagesByReviewId(review.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowReviewNotFoundException_WhenReviewDoesNotExist()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelReview?)null);
            
            var command = new UpdateReviewImagesCommand
            {
                ReviewId = 2,
                ReviewImages = new List<IFormFile>()
            };

            await Assert.ThrowsAsync<ReviewNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
