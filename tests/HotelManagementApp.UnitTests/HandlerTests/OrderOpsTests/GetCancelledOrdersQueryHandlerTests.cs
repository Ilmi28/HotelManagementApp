using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.OrderOps.GetCancelledOrders;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetCancelledOrdersQueryHandlerTests
    {
        private readonly Mock<ICancelledOrderRepository> _cancelledOrderRepositoryMock = new();
        private readonly Mock<IPendingOrderRepository> _pendingOrderRepositoryMock = new();
        private readonly Mock<IConfirmedOrderRepository> _confirmedOrderRepositoryMock = new();
        private readonly Mock<ICompletedOrderRepository> _completedOrderRepositoryMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly GetCancelledOrdersQueryHandler _handler;

        public GetCancelledOrdersQueryHandlerTests()
        {
            _handler = new GetCancelledOrdersQueryHandler(
                _cancelledOrderRepositoryMock.Object,
                _pendingOrderRepositoryMock.Object,
                _confirmedOrderRepositoryMock.Object,
                _completedOrderRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _pricingServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnCancelledOrders()
        {
            var order = new Order
            {
                Id = 1,
                UserId = "123",
                Status = Core.Enums.OrderStatusEnum.Cancelled,
                OrderDetails = new OrderDetails
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PhoneNumber = "123456789",
                    Address = "ul. Testowa 1",
                    City = "Warszawa",
                    Country = "Polska"
                }
            };
            var cancelledOrder = new CancelledOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2024, 1, 1)
            };

            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CancelledOrder> { cancelledOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PendingOrder?)null);
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfirmedOrder?)null);
            _completedOrderRepositoryMock.Setup(r => r.GetCompletedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CompletedOrder?)null);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(123.45m);

            var query = new GetCancelledOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            var response = Assert.Single(result);
            Assert.Equal(1, response.Id);
            Assert.Equal("123", response.UserId);
            Assert.Equal("Cancelled", response.Status);
            Assert.Equal("Jan", response.FirstName);
            Assert.Equal("Kowalski", response.LastName);
            Assert.Equal("123456789", response.PhoneNumber);
            Assert.Equal("ul. Testowa 1", response.Address);
            Assert.Equal("Warszawa", response.City);
            Assert.Equal("Polska", response.Country);
            Assert.Equal(new DateTime(2024, 1, 1), response.Cancelled);
            Assert.Equal(123.45m, response.TotalPrice);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoCancelledOrdersExist()
        {
            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CancelledOrder>());

            var query = new GetCancelledOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
