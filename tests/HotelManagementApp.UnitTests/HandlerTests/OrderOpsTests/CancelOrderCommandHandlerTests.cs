using HotelManagementApp.Application.CQRS.OrderOps.CancelOrder;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class CancelOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<ICancelledOrderRepository> _cancelledOrderRepositoryMock = new();
        private readonly CancelOrderCommandHandler _handler;

        public CancelOrderCommandHandlerTests()
        {
            _handler = new CancelOrderCommandHandler(
                _orderRepositoryMock.Object,
                _cancelledOrderRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldCancelOrder_WhenOrderIsActive()
        {
            var order = new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Pending,
                UserId = "123",
                Reservations = new List<Reservation>()
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _orderRepositoryMock.Setup(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _cancelledOrderRepositoryMock.Setup(r => r.AddCancelledOrder(It.IsAny<CancelledOrder>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CancelOrderCommand { OrderId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatusEnum.Cancelled, order.Status);
            _orderRepositoryMock.Verify(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            _cancelledOrderRepositoryMock.Verify(r => r.AddCancelledOrder(It.Is<CancelledOrder>(c => c.Order == order), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new CancelOrderCommand { OrderId = 2 };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(OrderStatusEnum.Cancelled)]
        [InlineData(OrderStatusEnum.Completed)]
        public async Task ShouldThrowInvalidOperationException_WhenOrderIsFinalState(OrderStatusEnum status)
        {
            var order = new Order
            {
                Id = 3,
                Status = status,
                UserId = "123",
                Reservations = new List<Reservation>()
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var command = new CancelOrderCommand { OrderId = 3 };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
