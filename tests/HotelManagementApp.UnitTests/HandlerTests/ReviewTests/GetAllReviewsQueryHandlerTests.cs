using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.GetAll;
using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewTests
{
    public class GetAllReviewsQueryHandlerTests
    {
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly Mock<IHotelImageRepository> _hotelImageRepositoryMock = new();
        private readonly Mock<IFileService> _fileServiceMock = new();
        private readonly GetAllReviewsQueryHandler _handler;

        public GetAllReviewsQueryHandlerTests()
        {
            _handler = new GetAllReviewsQueryHandler(
                _reviewRepositoryMock.Object,
                _hotelImageRepositoryMock.Object,
                _fileServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllReviews()
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
            var hotelImages = new List<HotelImage>
            {
                new HotelImage { Id = 1, FileName = "img1.jpg", Hotel = hotel },
                new HotelImage { Id = 2, FileName = "img2.jpg", Hotel = hotel }
            };

            _reviewRepositoryMock.Setup(r => r.GetAllReviews(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelReview> { review });
            _hotelImageRepositoryMock.Setup(r => r.GetHotelImagesByHotelId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotelImages);
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img1.jpg")).Returns("url1");
            _fileServiceMock.Setup(f => f.GetFileUrl("images", "img2.jpg")).Returns("url2");

            var query = new GetAllReviewsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            var response = result.First();
            Assert.Equal(1, response.Id);
            Assert.Equal(1, response.HotelId);
            Assert.Equal("guestuser", response.UserName);
            Assert.Equal(5, response.Rating);
            Assert.Equal("Great stay!", response.Review);
            Assert.Equal(new DateTime(2024, 1, 1), response.Created);
            Assert.Equal(new DateTime(2024, 1, 2), response.LastModified);
            Assert.Equal(2, response.ReviewImages.Count);
            Assert.Contains("url1", response.ReviewImages);
            Assert.Contains("url2", response.ReviewImages);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoReviews()
        {
            _reviewRepositoryMock.Setup(r => r.GetAllReviews(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HotelReview>());

            var query = new GetAllReviewsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
