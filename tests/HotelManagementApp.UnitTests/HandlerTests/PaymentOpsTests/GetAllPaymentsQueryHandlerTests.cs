using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetAllPayments;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetAllPaymentsQueryHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetAllPaymentsQueryHandler _handler;

        public GetAllPaymentsQueryHandlerTests()
        {
            _handler = new GetAllPaymentsQueryHandler(_paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllPayments()
        {
            var payments = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    PaymentMethod = PaymentMethodEnum.Cash,
                    OrderId = 10,
                    Amount = 100,
                    Date = new DateTime(2025, 1, 1)
                }
            };

            _paymentRepositoryMock.Setup(r => r.GetPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(payments);

            var query = new GetAllPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            Assert.Equal("CASH", result.First().PaymentMethod);
            Assert.Equal(10, result.First().OrderId);
            Assert.Equal(100, result.First().Amount);
            Assert.Equal(new DateTime(2025, 1, 1), result.First().Date);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoPayments()
        {
            _paymentRepositoryMock.Setup(r => r.GetPayments(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Payment>());

            var query = new GetAllPaymentsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
