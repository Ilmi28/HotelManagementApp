using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.OrderOps.GetPendingOrders;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetPendingOrdersQueryHandlerTests
    {
        private readonly Mock<IPendingOrderRepository> _pendingOrderRepositoryMock = new();
        private readonly Mock<IConfirmedOrderRepository> _confirmedOrderRepositoryMock = new();
        private readonly Mock<ICancelledOrderRepository> _cancelledOrderRepositoryMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly GetPendingOrdersQueryHandler _handler;

        public GetPendingOrdersQueryHandlerTests()
        {
            _handler = new GetPendingOrdersQueryHandler(
                _pendingOrderRepositoryMock.Object,
                _confirmedOrderRepositoryMock.Object,
                _cancelledOrderRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _pricingServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnPendingOrders_WhenOrdersArePending()
        {
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
            var pendingOrder = new PendingOrder
            {
                Id = 1,
                Date = new DateTime(2024, 1, 1),
                Order = order
            };

            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendingOrder> { pendingOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfirmedOrder?)null);
            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CancelledOrder?)null);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100m);

            var query = new GetPendingOrdersQuery();

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
            Assert.Equal(new DateTime(2024, 1, 1), response.Created);
            Assert.Equal(100m, response.TotalPrice);
        }

        [Fact]
        public async Task ShouldReturnEmpty_WhenNoPendingOrders()
        {
            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendingOrder>());

            var query = new GetPendingOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ShouldSkipOrder_WhenOrderIsNull()
        {
            var pendingOrder = new PendingOrder
            {
                Id = 1,
                Date = new DateTime(2024, 1, 1),
                Order = new Order
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
                }
            };

            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendingOrder> { pendingOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var query = new GetPendingOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ShouldSkipOrder_WhenOrderIsConfirmedOrCancelled()
        {
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
            var pendingOrder = new PendingOrder
            {
                Id = 1,
                Date = new DateTime(2024, 1, 1),
                Order = order
            };

            _pendingOrderRepositoryMock.Setup(r => r.GetPendingOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendingOrder> { pendingOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConfirmedOrder());
            _cancelledOrderRepositoryMock.Setup(r => r.GetCancelledOrderByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CancelledOrder?)null);

            var query = new GetPendingOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
