using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.PayWithCash;

public class PayWithCashCommandHandler(
    IOrderRepository orderRepository, 
    IPaymentRepository paymentRepository,
    ICashPaymentRepository cashPaymentRepository,
    IPricingService pricingService,
    ICompletedOrderRepository completedOrderRepository) : IRequestHandler<PayWithCashCommand>
{
    public async Task Handle(PayWithCashCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
                    ?? throw new OrderNotFoundException($"Order with id: {request.OrderId} not found.");
        if (order.Status is OrderStatusEnum.Pending or OrderStatusEnum.Cancelled or OrderStatusEnum.Completed)
            throw new InvalidOperationException($"Order status should be confirmed to perform payment. Current status: {order.Status}");
        var payment = new Payment
        {
            Order = order,
            PaymentMethod = PaymentMethodEnum.Cash,
            Amount = await pricingService.CalculatePriceForOrder(order, cancellationToken)
        };
        await paymentRepository.AddPayment(payment, cancellationToken);
        var cashPayment = new CashPayment
        {
            Payment = payment
        };
        await cashPaymentRepository.AddCashPayment(cashPayment, cancellationToken);
        order.Status = OrderStatusEnum.Completed;
        await orderRepository.UpdateOrder(order, cancellationToken);
        await completedOrderRepository.AddCompletedOrder(new CompletedOrder { Order = order }, cancellationToken);
    }
}