using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.PayWithCash;
using HotelManagementApp.Application.CQRS.PaymentOps.PayWithCreditCard;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class PayWithCashCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly Mock<ICashPaymentRepository> _cashPaymentRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly Mock<ICompletedOrderRepository> _completedOrderRepositoryMock = new();
        private readonly Mock<IBillProductService> _billProductServiceMock = new();
        private readonly PayWithCashCommandHandler _handler;

        public PayWithCashCommandHandlerTests()
        {
            _handler = new PayWithCashCommandHandler(
                _orderRepositoryMock.Object,
                _paymentRepositoryMock.Object,
                _cashPaymentRepositoryMock.Object,
                _pricingServiceMock.Object,
                _completedOrderRepositoryMock.Object,
                _billProductServiceMock.Object);
        }

        [Fact]
        public async Task ShouldPayWithCash_WhenOrderIsConfirmed()
        {
            var order = new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Confirmed,
                UserId = "123",
                Reservations = new List<Reservation>()
            };
            var command = new PayWithCashCommand
            {
                OrderId = 1
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100m);
            _paymentRepositoryMock.Setup(r => r.AddPayment(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _orderRepositoryMock.Setup(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _completedOrderRepositoryMock.Setup(r => r.AddCompletedOrder(It.IsAny<CompletedOrder>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _billProductServiceMock.Setup(r => r.AddBillProductsForOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _orderRepositoryMock.Verify(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            _billProductServiceMock.Verify(r => r.AddBillProductsForOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(OrderStatusEnum.Completed, order.Status);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new PayWithCashCommand
            {
                OrderId = 2
            };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(OrderStatusEnum.Pending)]
        [InlineData(OrderStatusEnum.Cancelled)]
        [InlineData(OrderStatusEnum.Completed)]
        public async Task ShouldThrowInvalidOperationException_WhenOrderStatusIsNotConfirmed(OrderStatusEnum status)
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

            var command = new PayWithCashCommand
            {
                OrderId = 3
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}

