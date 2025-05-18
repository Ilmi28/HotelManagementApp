using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentById;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetPaymentByIdQueryHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetPaymentByIdQueryHandler _handler;

        public GetPaymentByIdQueryHandlerTests()
        {
            _handler = new GetPaymentByIdQueryHandler(_paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnPayment_WhenExists()
        {
            var payment = new Payment
            {
                Id = 1,
                PaymentMethod = PaymentMethodEnum.Cash,
                OrderId = 10,
                Amount = 100,
                Date = new DateTime(2025, 1, 1)
            };

            _paymentRepositoryMock.Setup(r => r.GetPaymentById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(payment);

            var query = new GetPaymentByIdQuery { PaymentId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.Id);
            Assert.Equal("CASH", result.PaymentMethod);
            Assert.Equal(10, result.OrderId);
            Assert.Equal(100, result.Amount);
            Assert.Equal(new DateTime(2025, 1, 1), result.Date);
        }

        [Fact]
        public async Task ShouldThrowPaymentNotFoundException_WhenNotExists()
        {
            _paymentRepositoryMock.Setup(r => r.GetPaymentById(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Payment?)null);

            var query = new GetPaymentByIdQuery { PaymentId = 2 };

            await Assert.ThrowsAsync<PaymentNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
