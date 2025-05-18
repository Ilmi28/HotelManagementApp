using HotelManagementApp.Application.Interfaces;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.PayWithCreditCard;

public class PayWithCreditCardCommandHandler(
    IOrderRepository orderRepository,
    IPaymentRepository paymentRepository,
    ICreditCardPaymentRepository creditCardPaymentRepository,
    IPricingService pricingService,
    ICreditCardPaymentService creditCardPaymentService,
    ICompletedOrderRepository completedOrderRepository,
    IBillProductService billProductService) : IRequestHandler<PayWithCreditCardCommand>
{
    public async Task Handle(PayWithCreditCardCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
                    ?? throw new OrderNotFoundException($"Order with id: {request.OrderId} not found.");
        if (order.Status is OrderStatusEnum.Pending or OrderStatusEnum.Cancelled or OrderStatusEnum.Completed)
            throw new InvalidOperationException($"Order {order.Id} status should be confirmed to perform payment. Current status: {order.Status}");
        await creditCardPaymentService.Pay(request.CreditCardNumber, request.CreditCardCvv,
            request.CreditCardExpirationDate);
        var payment = new Payment
        {
            OrderId = order.Id,
            PaymentMethod = PaymentMethodEnum.CreditCard,
            Amount = await pricingService.CalculatePriceForOrder(order, cancellationToken)
        };
        await paymentRepository.AddPayment(payment, cancellationToken);
        var creditCardPayment = new CreditCardPayment
        {
            Payment = payment,
            CreditCardNumber = request.CreditCardNumber,
            CreditCardExpirationDate = request.CreditCardExpirationDate,
            CreditCardCvv = request.CreditCardCvv,
        };
        await creditCardPaymentRepository.AddCreditCardPayment(creditCardPayment, cancellationToken);
        order.Status = OrderStatusEnum.Completed;
        await orderRepository.UpdateOrder(order, cancellationToken);
        await completedOrderRepository.AddCompletedOrder(new CompletedOrder { Order = order, OrderId = order.Id }, cancellationToken);
        await billProductService.AddBillProductsForOrder(order, cancellationToken);
    }
}