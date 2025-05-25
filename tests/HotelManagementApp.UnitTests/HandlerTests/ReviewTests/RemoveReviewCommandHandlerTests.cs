using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.Remove;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewTests
{
    public class RemoveReviewCommandHandlerTests
    {
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly Mock<IHotelReviewImageRepository> _reviewImageRepositoryMock = new();
        private readonly RemoveReviewCommandHandler _handler;

        public RemoveReviewCommandHandlerTests()
        {
            _handler = new RemoveReviewCommandHandler(
                _reviewRepositoryMock.Object,
                _reviewImageRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldRemoveReview_WhenExists()
        {
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                UserName = "guestuser",
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "a@a", Description = "d" },
                Rating = 5,
                Review = "Test",
                Created = DateTime.Now,
                LastModified = DateTime.Now
            };

            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _reviewRepositoryMock.Setup(r => r.RemoveReview(review, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _reviewImageRepositoryMock.Setup(r => r.RemoveReviewImagesByReviewId(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new RemoveReviewCommand { ReviewId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            _reviewRepositoryMock.Verify(r => r.RemoveReview(review, It.IsAny<CancellationToken>()), Times.Once);
            _reviewImageRepositoryMock.Verify(r => r.RemoveReviewImagesByReviewId(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowReviewNotFoundException_WhenNotExists()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelReview?)null);

            var command = new RemoveReviewCommand { ReviewId = 2 };

            await Assert.ThrowsAsync<ReviewNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
