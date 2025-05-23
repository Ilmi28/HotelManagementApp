using HotelManagementApp.Application.CQRS.OrderOps.GetCompletedOrders;
using HotelManagementApp.Application.Dtos;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetCompletedOrdersQueryHandlerTests
    {
        private readonly Mock<ICompletedOrderRepository> _completedOrderRepositoryMock = new();
        private readonly Mock<IOrderStatusService> _orderStatusServiceMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetCompletedOrdersQueryHandler _handler;

        public GetCompletedOrdersQueryHandlerTests()
        {
            _handler = new GetCompletedOrdersQueryHandler(
                _completedOrderRepositoryMock.Object,
                _orderStatusServiceMock.Object,
                _orderRepositoryMock.Object,
                _paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnCompletedOrders()
        {
            var order = new Order
            {
                Id = 1,
                UserId = "123",
                Status = Core.Enums.OrderStatusEnum.Completed,
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
            var completedOrder = new CompletedOrder
            {
                Id = 1,
                OrderId = 1,
                Order = order,
                Date = new DateTime(2025, 1, 1)
            };
            var payment = new Payment
            {
                Id = 1,
                OrderId = 1,
                Order = order,
                Amount = 123.45m,
                PaymentMethod = Core.Enums.PaymentMethodEnum.Cash,
                Date = new DateTime(2025, 1, 1)
            };
            var orderStatuses = new OrderStatusesDto
            {
                OrderId = 1,
                CompletedDate = new DateTime(2025, 1, 1),
                ConfirmedDate = new DateTime(2025, 1, 1),
                CreatedDate = new DateTime(2025, 1, 1),
            };
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompletedOrder> { completedOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _orderStatusServiceMock.Setup(r => r.GetOrderStatusesAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderStatuses);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(payment);

            var query = new GetCompletedOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            var response = Assert.Single(result);
            Assert.Equal(1, response.Id);
            Assert.Equal("123", response.UserId);
            Assert.Equal("Completed", response.Status);
            Assert.Equal("Jan", response.FirstName);
            Assert.Equal("Kowalski", response.LastName);
            Assert.Equal("123456789", response.PhoneNumber);
            Assert.Equal("Testowa", response.Address);
            Assert.Equal("Warszawa", response.City);
            Assert.Equal("Polska", response.Country);
            Assert.Equal(new DateTime(2025, 1, 1), response.Completed);
            Assert.Equal(123.45m, response.TotalPrice);
        }

        [Fact]
        public async Task ShouldThrowPaymentNotFoundException_WhenPaymentDoesNotExist()
        {
            var order = new Order
            {
                Id = 2,
                UserId = "123",
                Status = Core.Enums.OrderStatusEnum.Completed,
                OrderDetails = new OrderDetails
                {
                    FirstName = "Anna",
                    LastName = "Nowak",
                    PhoneNumber = "987654321",
                    Address = "Testowa",
                    City = "Krak�w",
                    Country = "Polska"
                }
            };
            var completedOrder = new CompletedOrder
            {
                Id = 2,
                OrderId = 2,
                Order = order,
                Date = new DateTime(2025, 2, 2)
            };
            var orderStatuses = new OrderStatusesDto
            {
                OrderId = 2,
                CreatedDate = new DateTime(2025, 1, 1),
                ConfirmedDate = new DateTime(2025, 1, 1),
            };
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompletedOrder> { completedOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _orderStatusServiceMock.Setup(r => r.GetOrderStatusesAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderStatuses);
            _paymentRepositoryMock.Setup(r => r.GetPaymentsByOrderId(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Payment?)null);

            var query = new GetCompletedOrdersQuery();

            await Assert.ThrowsAsync<PaymentNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoCompletedOrdersExist()
        {
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompletedOrder>());

            var query = new GetCompletedOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

