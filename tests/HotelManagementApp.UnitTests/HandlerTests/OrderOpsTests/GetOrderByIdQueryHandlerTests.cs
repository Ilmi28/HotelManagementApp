using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrderById;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPendingOrderRepository> _pendingOrderRepositoryMock = new();
        private readonly Mock<ICompletedOrderRepository> _completedOrderRepositoryMock = new();
        private readonly Mock<IConfirmedOrderRepository> _confirmedOrderRepositoryMock = new();
        private readonly Mock<ICancelledOrderRepository> _cancelledOrderRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetOrderByIdQueryHandler _handler;

        public GetOrderByIdQueryHandlerTests()
        {
            _handler = new GetOrderByIdQueryHandler(
                _orderRepositoryMock.Object,
                _pendingOrderRepositoryMock.Object,
                _completedOrderRepositoryMock.Object,
                _confirmedOrderRepositoryMock.Object,
                _cancelledOrderRepositoryMock.Object,
                _pricingServiceMock.Object,
                _paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnOrder_WhenOrderExists()
        {
            var order = new Order
            {
                Id = 1,
                UserId = "123",
                Status = Core.Enums.OrderStatusEnum.Confirmed,
                OrderDetails = new OrderDetails
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "123456789",
                    Address = "Testowa",
                    City = "Warszawa",
                    Country = "Polska"
                }
            };
            var pendingOrder = new PendingOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2025, 1, 1)
            };
            var confirmedOrder = new ConfirmedOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2025, 1, 2)
            };
            var cancelledOrder = new CancelledOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2025, 1, 3)
            };
            var completedOrder = new CompletedOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2025, 1, 4)
            };
            var payment = new Payment
            {
                Id = 1,
                Order = order,
                Amount = 123.45m,
                PaymentMethod = Core.Enums.PaymentMethodEnum.Cash,
                Date = new DateTime(2025, 1, 5)
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingOrder);
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(confirmedOrder);
            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cancelledOrder);
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(completedOrder);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(payment);

            var query = new GetOrderByIdQuery { OrderId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("123", result.UserId);
            Assert.Equal("Confirmed", result.Status);
            Assert.Equal("Jan", result.FirstName);
            Assert.Equal("Kowalski", result.LastName);
            Assert.Equal("123456789", result.PhoneNumber);
            Assert.Equal("Testowa", result.Address);
            Assert.Equal("Warszawa", result.City);
            Assert.Equal("Polska", result.Country);
            Assert.Equal(new DateTime(2025, 1, 1), result.Created);
            Assert.Equal(new DateTime(2025, 1, 2), result.Confirmed);
            Assert.Equal(new DateTime(2025, 1, 3), result.Cancelled);
            Assert.Equal(new DateTime(2025, 1, 4), result.Completed);
            Assert.Equal(123.45m, result.TotalPrice);
        }

        [Fact]
        public async Task ShouldReturnOrderWithCalculatedPrice_WhenPaymentIsNull()
        {
            var order = new Order
            {
                Id = 2,
                UserId = "123",
                Status = Core.Enums.OrderStatusEnum.Pending,
                OrderDetails = new OrderDetails
                {
                    FirstName = "Anna",
                    LastName = "Nowak",
                    PhoneNumber = "987654321",
                    Address = "Testowa",
                    City = "Kraków",
                    Country = "Polska"
                }
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PendingOrder?)null);
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrderByOrderId(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfirmedOrder?)null);
            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrderByOrderId(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CancelledOrder?)null);
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrderByOrderId(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CompletedOrder?)null);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByOrderId(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Payment?)null);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(99.99m);

            var query = new GetOrderByIdQuery { OrderId = 2 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("123", result.UserId);
            Assert.Equal("Pending", result.Status);
            Assert.Equal("Anna", result.FirstName);
            Assert.Equal("Nowak", result.LastName);
            Assert.Equal("987654321", result.PhoneNumber);
            Assert.Equal("Testowa", result.Address);
            Assert.Equal("Kraków", result.City);
            Assert.Equal("Polska", result.Country);
            Assert.Null(result.Created);
            Assert.Null(result.Confirmed);
            Assert.Null(result.Cancelled);
            Assert.Null(result.Completed);
            Assert.Equal(99.99m, result.TotalPrice);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var query = new GetOrderByIdQuery { OrderId = 3 };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}

