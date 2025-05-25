using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.Review.Add;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.ReviewOpsTests
{
    public class AddReviewCommandHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IHotelReviewRepository> _reviewRepositoryMock = new();
        private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
        private readonly Mock<IReservationRepository> _reservationRepositoryMock = new();
        private readonly AddReviewCommandHandler _handler;

        public AddReviewCommandHandlerTests()
        {
            _handler = new AddReviewCommandHandler(
                _userManagerMock.Object,
                _reviewRepositoryMock.Object,
                _hotelRepositoryMock.Object,
                _reservationRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldAddReview_WhenUserHasCompletedReservation()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "guestuser",
                Email = "guest@example.com",
                Roles = new List<string> { "Guest" }
            };
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
            var reservation = new Reservation
            {
                Id = 1,
                From = new DateOnly(2023, 1, 1),
                To = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Room = new HotelRoom
                {
                    Id = 1,
                    RoomName = "101",
                    RoomType = RoomTypeEnum.Economy,
                    Price = 100,
                    Description = "desc",
                    Hotel = hotel
                },
                Order = new Order
                {
                    Id = 1,
                    Status = OrderStatusEnum.Completed,
                    UserId = "123",
                    Reservations = new List<Reservation>()
                }
            };
            var command = new AddReviewCommand
            {
                UserId = "123",
                UserName = "guestuser",
                HotelId = 1,
                Rating = 5,
                Review = "Great stay!"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _hotelRepositoryMock.Setup(h => h.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _reservationRepositoryMock.Setup(r => r.GetReservationsByGuestId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Reservation> { reservation });
            _reviewRepositoryMock.Setup(r => r.AddReview(It.IsAny<HotelReview>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _reviewRepositoryMock.Verify(r => r.AddReview(It.Is<HotelReview>(rev =>
                rev.UserId == "123" &&
                rev.UserName == "guestuser" &&
                rev.Hotel == hotel &&
                rev.Rating == 5 &&
                rev.Review == "Great stay!"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            _userManagerMock.Setup(u => u.FindByIdAsync("notfound"))
                .ReturnsAsync((UserDto?)null);

            var command = new AddReviewCommand
            {
                UserId = "notfound",
                UserName = "guestuser",
                HotelId = 1,
                Rating = 5,
                Review = "Great stay!"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowHotelNotFoundException_WhenHotelNotFound()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "guestuser",
                Email = "guest@example.com",
                Roles = new List<string> { "Guest" }
            };
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _hotelRepositoryMock.Setup(h => h.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hotel?)null);

            var command = new AddReviewCommand
            {
                UserId = "123",
                UserName = "guestuser",
                HotelId = 1,
                Rating = 5,
                Review = "Great stay!"
            };

            await Assert.ThrowsAsync<HotelNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowInvalidOperationException_WhenUserHasNoCompletedReservation()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "guestuser",
                Email = "guest@example.com",
                Roles = new List<string> { "Guest" }
            };
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
            var reservation = new Reservation
            {
                Id = 1,
                From = new DateOnly(2023, 1, 1),
                To = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Room = new HotelRoom
                {
                    Id = 1,
                    RoomName = "101",
                    RoomType = RoomTypeEnum.Economy,
                    Price = 100,
                    Description = "desc",
                    Hotel = hotel
                },
                Order = new Order
                {
                    Id = 1,
                    Status = OrderStatusEnum.Cancelled,
                    UserId = "123",
                    Reservations = new List<Reservation>()
                }
            };
            var command = new AddReviewCommand
            {
                UserId = "123",
                UserName = "guestuser",
                HotelId = 1,
                Rating = 5,
                Review = "Great stay!"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _hotelRepositoryMock.Setup(h => h.GetHotelById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(hotel);
            _reservationRepositoryMock.Setup(r => r.GetReservationsByGuestId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Reservation> { reservation });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
