using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPayments;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetCreditCardPaymentsQueryHandlerTests
    {
        private readonly Mock<ICreditCardPaymentRepository> _creditCardPaymentRepositoryMock = new();
        private readonly GetCreditCardPaymentsQueryHandler _handler;

        public GetCreditCardPaymentsQueryHandlerTests()
        {
            _handler = new GetCreditCardPaymentsQueryHandler(_creditCardPaymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllCreditCardPayments()
        {
            var payments = new List<CreditCardPayment>
            {
                new CreditCardPayment
                {
                    Id = 1,
                    CreditCardNumber = "1111222233334444",
                    CreditCardExpirationDate = "11/29",
                    CreditCardCvv = "111",
                    Payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 10, OrderId = 1, PaymentMethod = Core.Enums.PaymentMethodEnum.CreditCard }
                },
                new CreditCardPayment
                {
                    Id = 2,
                    CreditCardNumber = "5555666677778888",
                    CreditCardExpirationDate = "12/30",
                    CreditCardCvv = "222",
                    Payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 20, OrderId = 2, PaymentMethod = Core.Enums.PaymentMethodEnum.CreditCard }
                }
            };

            _creditCardPaymentRepositoryMock.Setup(r => r.GetCreditCardPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(payments);

            var query = new GetCreditCardPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.First().Id);
            Assert.Equal("1111222233334444", result.First().CreditCardNumber);
            Assert.Equal("11/29", result.First().CreditCardExpirationDate);
            Assert.Equal("111", result.First().CreditCardCvv);
            Assert.Equal(10, result.First().PaymentId);
            Assert.Equal(2, result.Last().Id);
            Assert.Equal("5555666677778888", result.Last().CreditCardNumber);
            Assert.Equal("12/30", result.Last().CreditCardExpirationDate);
            Assert.Equal("222", result.Last().CreditCardCvv);
            Assert.Equal(20, result.Last().PaymentId);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoCreditCardPayments()
        {
            _creditCardPaymentRepositoryMock.Setup(r => r.GetCreditCardPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CreditCardPayment>());

            var query = new GetCreditCardPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
