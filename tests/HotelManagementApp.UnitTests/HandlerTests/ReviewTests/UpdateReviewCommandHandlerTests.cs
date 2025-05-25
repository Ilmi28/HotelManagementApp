using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.Update;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewTests
{
    public class UpdateReviewCommandHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly UpdateReviewCommandHandler _handler;

        public UpdateReviewCommandHandlerTests()
        {
            _handler = new UpdateReviewCommandHandler(
                _userManagerMock.Object,
                _reviewRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateReview_WhenUserIsOwner()
        {
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                UserName = "olduser",
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "a@a", Description = "d" },
                Rating = 3,
                Review = "Old review",
                Created = DateTime.Now.AddDays(-2),
                LastModified = DateTime.Now.AddDays(-1)
            };
            var user = new UserDto
            {
                Id = "123",
                UserName = "newuser",
                Email = "user@example.com",
                Roles = new List<string> { "Guest" }
            };
            var command = new UpdateReviewCommand
            {
                ReviewId = 1,
                UserName = "newuser",
                Rating = 5,
                Review = "Updated review"
            };

            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _reviewRepositoryMock.Setup(r => r.UpdateReview(review, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Updated review", review.Review);
            Assert.Equal(5, review.Rating);
            Assert.Equal("newuser", review.UserName);
            _reviewRepositoryMock.Verify(r => r.UpdateReview(review, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowReviewNotFoundException_WhenReviewNotExists()
        {
            _reviewRepositoryMock.Setup(r => r.GetReviewById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HotelReview?)null);

            var command = new UpdateReviewCommand
            {
                ReviewId = 2,
                UserName = "user",
                Rating = 4,
                Review = "Test"
            };

            await Assert.ThrowsAsync<ReviewNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                UserName = "olduser",
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "a@a", Description = "d" },
                Rating = 3,
                Review = "Old review",
                Created = DateTime.Now.AddDays(-2),
                LastModified = DateTime.Now.AddDays(-1)
            };
            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync((UserDto?)null);

            var command = new UpdateReviewCommand
            {
                ReviewId = 1,
                UserName = "user",
                Rating = 4,
                Review = "Test"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserIsNotOwner()
        {
            var review = new HotelReview
            {
                Id = 1,
                UserId = "123",
                UserName = "olduser",
                Hotel = new Hotel { Id = 1, Name = "Test", Address = "A", City = new City { Id = 1, Name = "C", Country = "PL", Latitude = 0, Longitude = 0 }, PhoneNumber = "1", Email = "a@a", Description = "d" },
                Rating = 3,
                Review = "Old review",
                Created = DateTime.Now.AddDays(-2),
                LastModified = DateTime.Now.AddDays(-1)
            };
            var user = new UserDto
            {
                Id = "999",
                UserName = "otheruser",
                Email = "user@example.com",
                Roles = new List<string> { "Guest" }
            };
            _reviewRepositoryMock.Setup(r => r.GetReviewById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);

            var command = new UpdateReviewCommand
            {
                ReviewId = 1,
                UserName = "user",
                Rating = 4,
                Review = "Test"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
