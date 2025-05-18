using HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;
using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUserManager> _userManagerMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPendingOrderRepository> _pendingOrderRepositoryMock = new();
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _handler = new CreateOrderCommandHandler(
                _userManagerMock.Object,
                _orderRepositoryMock.Object,
                _pendingOrderRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldCreateOrder_WhenUserIsGuest()
        {
            var user = new UserDto
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { "Guest" }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);
            _orderRepositoryMock.Setup(r => r.AddOrder(It.IsAny<Order>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _pendingOrderRepositoryMock.Setup(r => r.AddPendingOrder(It.IsAny<PendingOrder>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new CreateOrderCommand
            {
                UserId = "123",
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "123456789",
                Address = "Testowa",
                City = "Warszawa",
                Country = "Polska"
            };

            await _handler.Handle(command, CancellationToken.None);

            _orderRepositoryMock.Verify(r => r.AddOrder(It.Is<Order>(o =>
                o.UserId == "123" &&
                o.Status == OrderStatusEnum.Pending &&
                o.OrderDetails.FirstName == "Jan" &&
                o.OrderDetails.LastName == "Kowalski" &&
                o.OrderDetails.PhoneNumber == "123456789" &&
                o.OrderDetails.Address == "Testowa" &&
                o.OrderDetails.City == "Warszawa" &&
                o.OrderDetails.Country == "Polska"
            ), It.IsAny<CancellationToken>()), Times.Once);

            _pendingOrderRepositoryMock.Verify(r => r.AddPendingOrder(It.IsAny<PendingOrder>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorizedAccessException_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync((UserDto?)null);

            var command = new CreateOrderCommand
            {
                UserId = "123",
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "123456789",
                Address = "Testowa",
                City = "Warszawa",
                Country = "Polska"
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
                UserName = "testuser",
                Email = "test@example.com",
                Roles = new List<string> { }
            };

            _userManagerMock.Setup(m => m.FindByIdAsync("123")).ReturnsAsync(user);

            var command = new CreateOrderCommand
            {
                UserId = "123",
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "123456789",
                Address = "ul. Testowa 1",
                City = "Warszawa",
                Country = "Polska"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
