using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPayments;

public class GetCashPaymentsQueryHandler(ICashPaymentRepository cashPaymentRepository) 
    : IRequestHandler<GetCashPaymentsQuery, ICollection<CashPaymentResponse>>
{
    public async Task<ICollection<CashPaymentResponse>> Handle(GetCashPaymentsQuery request, CancellationToken cancellationToken)
    {
        var cashPayments = await cashPaymentRepository.GetCashPayments(cancellationToken);
    
        return cashPayments.Select(x => new CashPaymentResponse
        {
            Id = x.Id,
            PaymentId = x.Payment.Id
        }).ToList();
    }
}