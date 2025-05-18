using HotelManagementApp.Application.CQRS.OrderOps.GetOrdersByGuest;
using HotelManagementApp.Application.Dtos;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetOrdersByGuestQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IOrderStatusService> _orderStatusServiceMock = new();
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetOrdersByGuestQueryHandler _handler;

        public GetOrdersByGuestQueryHandlerTests()
        {
            _handler = new GetOrdersByGuestQueryHandler(
                _orderRepositoryMock.Object,
                _userManagerMock.Object,
                _pricingServiceMock.Object,
                _paymentRepositoryMock.Object,
                _orderStatusServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnOrders_WhenUserIsGuest()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "guestuser",
                Email = "guest@example.com",
                Roles = new List<string> { "Guest" }
            };
            var order = new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Pending,
                UserId = "123",
                Reservations = new List<Reservation>(),
                OrderDetails = new OrderDetails
                {
                    Address = "A",
                    City = "B",
                    Country = "C",
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "123456789"
                }
            };
            var orderStatuses = new OrderStatusesDto
            {
                OrderId = 1
            };
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _orderRepositoryMock.Setup(r => r.GetOrdersByGuestId("123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Order> { order });
            _orderStatusServiceMock.Setup(r => r.GetOrderStatusesAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderStatuses);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Payment?)null);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100m);

            var query = new GetOrdersByGuestQuery { GuestId = "123" };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            var response = result.First();
            Assert.Equal(1, response.Id);
            Assert.Equal("123", response.UserId);
            Assert.Equal("Pending", response.Status);
            Assert.Equal("Jan", response.FirstName);
            Assert.Equal("Kowalski", response.LastName);
            Assert.Equal("A", response.Address);
            Assert.Equal("B", response.City);
            Assert.Equal("C", response.Country);
            Assert.Equal("123456789", response.PhoneNumber);
            Assert.Equal(100m, response.TotalPrice);
        }

        [Fact]
        public async Task ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync((UserDto?)null);

            var query = new GetOrdersByGuestQuery { GuestId = "123" };

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowInvalidOperationException_WhenUserIsNotGuest()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "admin",
                Email = "admin@example.com",
                Roles = new List<string> { "Admin" }
            };
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);

            var query = new GetOrdersByGuestQuery { GuestId = "123" };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
