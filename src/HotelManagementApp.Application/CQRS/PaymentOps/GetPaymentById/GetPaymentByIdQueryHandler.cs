using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentById;

public class GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentByIdQuery, PaymentResponse>
{
    public async Task<PaymentResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetPaymentById(request.PaymentId, cancellationToken)
            ?? throw new PaymentNotFoundException($"Payment with id: {request.PaymentId} not found.");
        return new PaymentResponse
        {
            Id = payment.Id,
            PaymentMethod = payment.PaymentMethod.ToString().ToUpper(),
            Date = payment.Date,
            Amount = payment.Amount,
            OrderId = payment.Order.Id
        };
    }
}