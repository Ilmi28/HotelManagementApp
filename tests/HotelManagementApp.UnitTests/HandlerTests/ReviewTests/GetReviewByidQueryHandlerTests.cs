using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.GetById;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewTests
{
    public class GetReviewByidQueryHandlerTests
    {
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly Mock<IHotelReviewImageRepository> _reviewImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly GetReviewByidQueryHandler _handler;

        public GetReviewByidQueryHandlerTests()
        {
            _handler = new GetReviewByidQueryHandler(
                _reviewRepositoryMock.Object,
                _reviewImageRepositoryMock.Object,
                _fileServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnReview_WhenExists()
        {
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "Test Address",
                City = new City { Id = 1, Name = "Test City", Country = "Test Country", Latitude = 0, Longitude = 0 },
                PhoneNumber = "123456789",
                Email = "hotel@example.com",
                Description = "desc"
            };
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                UserName = "guestuser",
                Hotel = hotel,
                Rating = 5,
                Review = "Great stay!",
                Created = new DateTime(2024, 1, 1),
                LastModified = new DateTime(2024, 1, 2)
            };
            var reviewImages = new List<HotelReviewImage>
            {
                new HotelReviewImage { Id = 1, FileName = "img1.jpg", HotelReview = review },
                new HotelReviewImage { Id = 2, FileName = "img2.jpg", HotelReview = review }
            };

            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _reviewImageRepositoryMock.Setup(r => r.GetReviewImagesByReviewId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reviewImages);
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img1.jpg")).Returns("url1");
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img2.jpg")).Returns("url2");

            var query = new GetReviewByIdQuery { ReviewId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.HotelId);
            Assert.Equal("guestuser", result.UserName);
            Assert.Equal(5, result.Rating);
            Assert.Equal("Great stay!", result.Review);
            Assert.Equal(new DateTime(2024, 1, 1), result.Created);
            Assert.Equal(new DateTime(2024, 1, 2), result.LastModified);
            Assert.Equal(2, result.ReviewImages.Count);
            Assert.Contains("url1", result.ReviewImages);
            Assert.Contains("url2", result.ReviewImages);
        }

        [Fact]
        public async Task ShouldThrowReviewNotFoundException_WhenNotExists()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelReview?)null);

            var query = new GetReviewByIdQuery { ReviewId = 2 };

            await Assert.ThrowsAsync<ReviewNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
