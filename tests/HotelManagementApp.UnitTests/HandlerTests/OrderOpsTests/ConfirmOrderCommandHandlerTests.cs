using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.OrderOps.ConfirmOrder;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class ConfirmOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IConfirmedOrderRepository> _confirmedOrderRepositoryMock = new();
        private readonly ConfirmOrderCommandHandler _handler;

        public ConfirmOrderCommandHandlerTests()
        {
            _handler = new ConfirmOrderCommandHandler(
                _orderRepositoryMock.Object,
                _confirmedOrderRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldConfirmOrder_WhenOrderIsPending()
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
            _confirmedOrderRepositoryMock.Setup(r => r.AddConfirmedOrder(It.IsAny<ConfirmedOrder>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new ConfirmOrderCommand { OrderId = 1 };

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatusEnum.Confirmed, order.Status);
            _orderRepositoryMock.Verify(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            _confirmedOrderRepositoryMock.Verify(r => r.AddConfirmedOrder(It.Is<ConfirmedOrder>(c => c.Order == order), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new ConfirmOrderCommand { OrderId = 2 };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(OrderStatusEnum.Cancelled)]
        [InlineData(OrderStatusEnum.Completed)]
        [InlineData(OrderStatusEnum.Confirmed)]
        public async Task ShouldThrowInvalidOperationException_WhenOrderIsNotPending(OrderStatusEnum status)
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

            var command = new ConfirmOrderCommand { OrderId = 3 };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
