using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPayments;

public class GetCreditCardPaymentsQueryHandler(ICreditCardPaymentRepository creditCardPaymentRepository) 
    : IRequestHandler<GetCreditCardPaymentsQuery, ICollection<CreditCardPaymentResponse>>
{
    public async Task<ICollection<CreditCardPaymentResponse>> Handle(GetCreditCardPaymentsQuery request, CancellationToken cancellationToken)
    {
        var creditCardPayments = await creditCardPaymentRepository.GetCreditCardPayments(cancellationToken);
        var response = new List<CreditCardPaymentResponse>();
        
        foreach (var payment in creditCardPayments)
        {
            response.Add(new CreditCardPaymentResponse
            {
                Id = payment.Id,
                CreditCardNumber = payment.CreditCardNumber,
                CreditCardExpirationDate = payment.CreditCardExpirationDate,
                CreditCardCvv = payment.CreditCardCvv,
                PaymentId = payment.Payment.Id
            });
        }
        
        return response;
    }
}