using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCashPayments;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetCashPaymentsQueryHandlerTests
    {
        private readonly Mock<ICashPaymentRepository> _cashPaymentRepositoryMock = new();
        private readonly GetCashPaymentsQueryHandler _handler;

        public GetCashPaymentsQueryHandlerTests()
        {
            _handler = new GetCashPaymentsQueryHandler(_cashPaymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllCashPayments()
        {
            var payments = new List<CashPayment>
            {
                new CashPayment { Id = 1, Payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 10, OrderId = 1, PaymentMethod = Core.Enums.PaymentMethodEnum.Cash } },
                new CashPayment { Id = 2, Payment = new Payment { Amount = 100, Date = DateTime.Now, Id = 20, OrderId = 1, PaymentMethod = Core.Enums.PaymentMethodEnum.Cash} }
            };

            _cashPaymentRepositoryMock.Setup(r => r.GetCashPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(payments);

            var query = new GetCashPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.First().Id);
            Assert.Equal(10, result.First().PaymentId);
            Assert.Equal(2, result.Last().Id);
            Assert.Equal(20, result.Last().PaymentId);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoCashPayments()
        {
            _cashPaymentRepositoryMock.Setup(r => r.GetCashPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CashPayment>());

            var query = new GetCashPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
