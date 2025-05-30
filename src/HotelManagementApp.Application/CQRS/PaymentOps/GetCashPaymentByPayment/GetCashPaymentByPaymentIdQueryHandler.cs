using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPaymentByPayment;

public class GetCashPaymentByPaymentIdQueryHandler(
    ICashPaymentRepository cashPaymentRepository) : IRequestHandler<GetCashPaymentByPaymentIdQuery, CashPaymentResponse>
{
    public async Task<CashPaymentResponse> Handle(GetCashPaymentByPaymentIdQuery request, CancellationToken cancellationToken)
    {
        var cashPayment = await cashPaymentRepository.GetCashPaymentByPaymentId(request.PaymentId, cancellationToken)
            ?? throw new PaymentNotFoundException($"Cash payment with payment id {request.PaymentId} not found");
            
        return new CashPaymentResponse
        {
            Id = cashPayment.Id,
            PaymentId = cashPayment.Payment.Id
        };
    }
}