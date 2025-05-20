using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    public class PayWithCreditCardCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly Mock<ICreditCardPaymentRepository> _creditCardPaymentRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly Mock<ICreditCardPaymentService> _creditCardPaymentServiceMock = new();
        private readonly Mock<ICompletedOrderRepository> _completedOrderRepositoryMock = new();
        private readonly Mock<IBillProductService> _billProductServiceMock = new();
        private readonly PayWithCreditCardCommandHandler _handler;

        public PayWithCreditCardCommandHandlerTests()
        {
            _handler = new PayWithCreditCardCommandHandler(
                _orderRepositoryMock.Object,
                _paymentRepositoryMock.Object,
                _creditCardPaymentRepositoryMock.Object,
                _pricingServiceMock.Object,
                _creditCardPaymentServiceMock.Object,
                _completedOrderRepositoryMock.Object,
                _billProductServiceMock.Object);
        }

        [Fact]
        public async Task ShouldPayWithCreditCard_WhenOrderIsConfirmed()
        {
            var order = new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Confirmed,
                UserId = "123",
                Reservations = new List<Reservation>()
            };
            var command = new PayWithCreditCardCommand
            {
                OrderId = 1,
                CreditCardNumber = "1234567890123456",
                CreditCardExpirationDate = "12/30",
                CreditCardCvv = "123"
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _creditCardPaymentServiceMock.Setup(s => s.Pay(command.CreditCardNumber, command.CreditCardCvv, command.CreditCardExpirationDate))
                .Returns(Task.CompletedTask);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100m);
            _paymentRepositoryMock.Setup(r => r.AddPayment(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _creditCardPaymentRepositoryMock.Setup(r => r.AddCreditCardPayment(It.IsAny<CreditCardPayment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _orderRepositoryMock.Setup(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _completedOrderRepositoryMock.Setup(r => r.AddCompletedOrder(It.IsAny<CompletedOrder>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _billProductServiceMock.Setup(r => r.AddBillProductsForOrder(order, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _creditCardPaymentServiceMock.Verify(s => s.Pay(command.CreditCardNumber, command.CreditCardCvv, command.CreditCardExpirationDate), Times.Once);
            _paymentRepositoryMock.Verify(r => r.AddPayment(It.Is<Payment>(p =>
                p.OrderId == 1 &&
                p.PaymentMethod == PaymentMethodEnum.CreditCard &&
                p.Amount == 100m
            ), It.IsAny<CancellationToken>()), Times.Once);
            _creditCardPaymentRepositoryMock.Verify(r => r.AddCreditCardPayment(It.Is<CreditCardPayment>(c =>
                c.CreditCardNumber == command.CreditCardNumber &&
                c.CreditCardExpirationDate == command.CreditCardExpirationDate &&
                c.CreditCardCvv == command.CreditCardCvv
            ), It.IsAny<CancellationToken>()), Times.Once);
            _orderRepositoryMock.Verify(r => r.UpdateOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            _completedOrderRepositoryMock.Verify(r => r.AddCompletedOrder(It.Is<CompletedOrder>(c => c.Order == order && c.OrderId == order.Id), It.IsAny<CancellationToken>()), Times.Once);
            _billProductServiceMock.Verify(r => r.AddBillProductsForOrder(order, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(OrderStatusEnum.Completed, order.Status);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new PayWithCreditCardCommand
            {
                OrderId = 2,
                CreditCardNumber = "1234567890123456",
                CreditCardExpirationDate = "12/30",
                CreditCardCvv = "123"
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

            var command = new PayWithCreditCardCommand
            {
                OrderId = 3,
                CreditCardNumber = "1234567890123456",
                CreditCardExpirationDate = "12/30",
                CreditCardCvv = "123"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}

