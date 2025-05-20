using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCashPaymentByPayment;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetCashPaymentByPaymentIdQueryHandlerTests
    {
        private readonly Mock<ICashPaymentRepository> _cashPaymentRepositoryMock = new();
        private readonly GetCashPaymentByPaymentIdQueryHandler _handler;

        public GetCashPaymentByPaymentIdQueryHandlerTests()
        {
            _handler = new GetCashPaymentByPaymentIdQueryHandler(_cashPaymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnCashPayment_WhenExists()
        {
            var payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 5, OrderId = 1, PaymentMethod = Core.Enums.PaymentMethodEnum.Cash };
            var cashPayment = new CashPayment { Id = 10, Payment = payment };

            _cashPaymentRepositoryMock.Setup(r => r.GetCashPaymentByPaymentId(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cashPayment);

            var query = new GetCashPaymentByPaymentIdQuery { PaymentId = 5 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(10, result.Id);
            Assert.Equal(5, result.PaymentId);
        }

        [Fact]
        public async Task ShouldThrowPaymentNotFoundException_WhenNotExists()
        {
            _cashPaymentRepositoryMock.Setup(r => r.GetCashPaymentByPaymentId(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CashPayment?)null);

            var query = new GetCashPaymentByPaymentIdQuery { PaymentId = 7 };

            await Assert.ThrowsAsync<PaymentNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
