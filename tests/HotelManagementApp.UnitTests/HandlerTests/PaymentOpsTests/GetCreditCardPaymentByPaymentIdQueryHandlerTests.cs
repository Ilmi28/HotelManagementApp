using System;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPaymentByPayment;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetCreditCardPaymentByPaymentIdQueryHandlerTests
    {
        private readonly Mock<ICreditCardPaymentRepository> _creditCardPaymentRepositoryMock = new();
        private readonly GetCreditCardPaymentByPaymentIdQueryHandler _handler;

        public GetCreditCardPaymentByPaymentIdQueryHandlerTests()
        {
            _handler = new GetCreditCardPaymentByPaymentIdQueryHandler(_creditCardPaymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnCreditCardPayment_WhenExists()
        {
            var payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 5, OrderId = 1, PaymentMethod = Core.Enums.PaymentMethodEnum.CreditCard };
            var creditCardPayment = new CreditCardPayment
            {
                Id = 10,
                CreditCardNumber = "1234567890123456",
                CreditCardExpirationDate = "12/30",
                CreditCardCvv = "123",
                Payment = payment
            };

            _creditCardPaymentRepositoryMock.Setup(r => r.GetCreditCardPaymentById(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(creditCardPayment);

            var query = new GetCreditCardPaymentByPaymentIdQuery { PaymentId = 5 };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(10, result.Id);
            Assert.Equal("1234567890123456", result.CreditCardNumber);
            Assert.Equal("12/30", result.CreditCardExpirationDate);
            Assert.Equal("123", result.CreditCardCvv);
            Assert.Equal(5, result.PaymentId);
        }

        [Fact]
        public async Task ShouldThrowPaymentNotFoundException_WhenNotExists()
        {
            _creditCardPaymentRepositoryMock.Setup(r => r.GetCreditCardPaymentById(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CreditCardPayment?)null);

            var query = new GetCreditCardPaymentByPaymentIdQuery { PaymentId = 7 };

            await Assert.ThrowsAsync<PaymentNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
