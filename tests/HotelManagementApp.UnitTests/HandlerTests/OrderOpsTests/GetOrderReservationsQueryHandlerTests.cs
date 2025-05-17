using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.OrderOps.GetOrderReservations;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.OrderOpsTests
{
    public class GetOrderReservationsQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<IReservationRepository> _reservationRepositoryMock = new();
        private readonly GetOrderReservationsQueryHandler _handler;

        public GetOrderReservationsQueryHandlerTests()
        {
            _handler = new GetOrderReservationsQueryHandler(
                _orderRepositoryMock.Object,
                _reservationRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnReservations_WhenOrderExists()
        {
            var order = new Order
            {
                Id = 1,
                Status = OrderStatusEnum.Pending,
                UserId = "123",
                Reservations = new List<Reservation>()
            };
            var city = new City
            {
                Id = 1,
                Name = "Test City",
                Latitude = 50.0,
                Longitude = 20.0,
                Country = "Test Country"
            };
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Address = "123 Test Street",
                City = city,
                PhoneNumber = "123-456-7890",
                Email = "test@example.com",
                Description = "A test hotel description."
            };
            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = 10,
                    From = new DateOnly(2024, 1, 1),
                    To = new DateOnly(2024, 1, 2),
                    Room = new HotelRoom
                {
                    Id = 1,
                    Hotel = hotel,
                    RoomName = "Room 101",
                    RoomType = RoomTypeEnum.Premium,
                    Price = 200,
                    Description = "A test room description."
                },
                    Order = order
                }
            };

            _orderRepositoryMock.Setup(r => r.GetOrderById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _reservationRepositoryMock.Setup(r => r.GetReservationsByOrderId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservations);

            var query = new GetOrderReservationsQuery { OrderId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(10, result.First().Id);
            Assert.Equal(1, result.First().RoomId);
            Assert.Equal("123", result.First().UserId);
            Assert.Equal(1, result.First().OrderId);
        }

        [Fact]
        public async Task ShouldThrowOrderNotFoundException_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock.Setup(r => r.GetOrderById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var query = new GetOrderReservationsQuery { OrderId = 2 };

            await Assert.ThrowsAsync<OrderNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
