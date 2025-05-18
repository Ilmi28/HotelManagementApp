using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentMethods;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using Moq;
using Xunit;

namespace HotelManagementApp.UnitTests.HandlerTests.PaymentOpsTests
{
    public class GetPaymentMethodsQueryHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly GetPaymentMethodsQueryHandler _handler;

        public GetPaymentMethodsQueryHandlerTests()
        {
            _handler = new GetPaymentMethodsQueryHandler(_paymentRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllPaymentMethods()
        {
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod { Id = 1, Name = "Cash" },
                new PaymentMethod { Id = 2, Name = "Card" }
            };

            _paymentRepositoryMock.Setup(r => r.GetPaymentMethods(It.IsAny<CancellationToken>()))
                .ReturnsAsync(paymentMethods);

            var query = new GetPaymentMethodsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.First().Id);
            Assert.Equal("Cash", result.First().Name);
            Assert.Equal(2, result.Last().Id);
            Assert.Equal("Card", result.Last().Name);
        }

        [Fact]
        public async Task ShouldReturnEmptyList_WhenNoPaymentMethods()
        {
            _paymentRepositoryMock.Setup(r => r.GetPaymentMethods(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentMethod>());

            var query = new GetPaymentMethodsQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
