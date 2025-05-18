using HotelManagementApp.Application.CQRS.OrderOps.GetConfirmedOrders;
using HotelManagementApp.Application.Dtos;
using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetConfirmedOrdersQueryHandlerTests
    {
        private readonly Mock<IConfirmedOrderRepository> _confirmedOrderRepositoryMock = new();
        private readonly Mock<IOrderStatusService> _orderStatusServiceMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IPricingService> _pricingServiceMock = new();
        private readonly GetConfirmedOrdersQueryHandler _handler;

        public GetConfirmedOrdersQueryHandlerTests()
        {
            _handler = new GetConfirmedOrdersQueryHandler(
                _confirmedOrderRepositoryMock.Object,
                _orderStatusServiceMock.Object,
                _orderRepositoryMock.Object,
                _pricingServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnConfirmedOrders()
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
            var confirmedOrder = new ConfirmedOrder
            {
                Id = 1,
                Order = order,
                Date = new DateTime(2024, 1, 1)
            };
            var orderStatuses = new OrderStatusesDto
            {
                OrderId = 1,
                CreatedDate = new DateTime(2025, 1, 1),
                ConfirmedDate = new DateTime(2025, 1, 2),
            };
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConfirmedOrder> { confirmedOrder });
            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _orderStatusServiceMock.Setup(r => r.GetOrderStatusesAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderStatuses);
            _pricingServiceMock.Setup(p => p.CalculatePriceForOrder(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(123.45m);

            var query = new GetConfirmedOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            var response = Assert.Single(result);
            Assert.Equal(1, response.Id);
            Assert.Equal("123", response.UserId);
            Assert.Equal("Confirmed", response.Status);
            Assert.Equal("Jan", response.FirstName);
            Assert.Equal("Kowalski", response.LastName);
            Assert.Equal("123456789", response.PhoneNumber);
            Assert.Equal("Testowa", response.Address);
            Assert.Equal("Warszawa", response.City);
            Assert.Equal("Polska", response.Country);
            Assert.Equal(new DateTime(2025, 1, 2), response.Confirmed);
            Assert.Equal(123.45m, response.TotalPrice);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoConfirmedOrdersExist()
        {
            _confirmedOrderRepositoryMock.Setup(r => r.GetConfirmedOrders(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConfirmedOrder>());

            var query = new GetConfirmedOrdersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

