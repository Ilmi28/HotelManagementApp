using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentByOrder;

public class GetPaymentByOrderIdQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentByOrderIdQuery, PaymentResponse>
{
    public async Task<PaymentResponse> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetPaymentsByOrderId(request.OrderId, cancellationToken)
            ?? throw new PaymentNotFoundException($"Payment with order id: {request.OrderId} not found.");
        return new PaymentResponse
        {
            Id = payment.Id,
            PaymentMethod = payment.PaymentMethod.ToString().ToUpper(),
            Date = payment.Date,
            Amount = payment.Amount,
            OrderId = payment.OrderId
        };
    }
}