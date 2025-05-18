using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentsByGuest;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetPaymentsByGuestIdQueryHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetPaymentsByGuestIdQueryHandler _handler;

        public GetPaymentsByGuestIdQueryHandlerTests()
        {
            _handler = new GetPaymentsByGuestIdQueryHandler(
                _userManagerMock.Object,
                _paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnPayments_WhenUserExists()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "guestuser",
                Email = "guest@example.com",
                Roles = new List<string> { "Guest" }
            };
            var payments = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    PaymentMethod = PaymentMethodEnum.Cash,
                    OrderId = 10,
                    Amount = 100,
                    Date = new DateTime(2025, 1, 1)
                }
            };

            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByUserId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(payments);

            var query = new GetPaymentsByGuestIdQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            var response = result.First();
            Assert.Equal(1, response.Id);
            Assert.Equal("CASH", response.PaymentMethod);
            Assert.Equal(10, response.OrderId);
            Assert.Equal(100, response.Amount);
            Assert.Equal(new DateTime(2025, 1, 1), response.Date);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoPayments()
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
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByUserId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Payment>());

            var query = new GetPaymentsByGuestIdQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ShouldThrowUserNotFoundException_WhenUserNotFound()
        {
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync((UserDto?)null);

            var query = new GetPaymentsByGuestIdQuery { GuestId = "123" };

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
