using HotelManagementApp.Application.CQRS.OrderOps.UpdateOrder;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class UpdateOrderCommandHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly UpdateOrderCommandHandler _handler;

        public UpdateOrderCommandHandlerTests()
        {
            _handler = new UpdateOrderCommandHandler(
                _userManagerMock.Object,
                _orderRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldUpdateOrder_WhenDataIsValid()
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
            var command = new UpdateOrderCommand
            {
                OrderId = 1,
                UserId = "123",
                FirstName = "Adam",
                LastName = "Nowak",
                PhoneNumber = "987654321",
                Address = "New Address",
                City = "New City",
                Country = "New Country"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _orderRepositoryMock.Setup(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Adam", order.OrderDetails.FirstName);
            Assert.Equal("Nowak", order.OrderDetails.LastName);
            Assert.Equal("987654321", order.OrderDetails.PhoneNumber);
            Assert.Equal("New Address", order.OrderDetails.Address);
            Assert.Equal("New City", order.OrderDetails.City);
            Assert.Equal("New Country", order.OrderDetails.Country);
            _orderRepositoryMock.Verify(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            _userManagerMock.Setup(u => u.FindByIdAsync("notfound"))
                .ReturnsAsync((UserDto?)null);

            var command = new UpdateOrderCommand
            {
                OrderId = 1,
                UserId = "notfound",
                FirstName = "Adam",
                LastName = "Nowak",
                PhoneNumber = "987654321",
                Address = "New Address",
                City = "New City",
                Country = "New Country"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
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

            var command = new UpdateOrderCommand
            {
                OrderId = 1,
                UserId = "123",
                FirstName = "Adam",
                LastName = "Nowak",
                PhoneNumber = "987654321",
                Address = "New Address",
                City = "New City",
                Country = "New Country"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderNotFound()
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
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new UpdateOrderCommand
            {
                OrderId = 1,
                UserId = "123",
                FirstName = "Adam",
                LastName = "Nowak",
                PhoneNumber = "987654321",
                Address = "New Address",
                City = "New City",
                Country = "New Country"
            };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(OrderStatusEnum.Cancelled)]
        [InlineData(OrderStatusEnum.Completed)]
        [InlineData(OrderStatusEnum.Confirmed)]
        public async Task ShouldThrowInvalidOperationException_WhenOrderIsNotPending(OrderStatusEnum status)
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
                Status = status,
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
            _userManagerMock.Setup(u => u.FindByIdAsync("123"))
                .ReturnsAsync(user);
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var command = new UpdateOrderCommand
            {
                OrderId = 1,
                UserId = "123",
                FirstName = "Adam",
                LastName = "Nowak",
                PhoneNumber = "987654321",
                Address = "New Address",
                City = "New City",
                Country = "New Country"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
