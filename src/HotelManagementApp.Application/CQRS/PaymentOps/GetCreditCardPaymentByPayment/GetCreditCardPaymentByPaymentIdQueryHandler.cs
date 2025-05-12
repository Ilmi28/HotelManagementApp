using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPaymentByPayment;

public class GetCreditCardPaymentByPaymentIdQueryHandler(ICreditCardPaymentRepository creditCardPaymentRepository) 
    : IRequestHandler<GetCreditCardPaymentByPaymentIdQuery, CreditCardPaymentResponse>
{
    public async Task<CreditCardPaymentResponse> Handle(GetCreditCardPaymentByPaymentIdQuery request, CancellationToken cancellationToken)
    {
        var creditCardPayment =
            await creditCardPaymentRepository.GetCreditCardPaymentById(request.PaymentId, cancellationToken)
            ?? throw new PaymentNotFoundException($"Credit card payment with payment id {request.PaymentId} not found");
    
        return new CreditCardPaymentResponse
        {
            Id = creditCardPayment.Id,
            CreditCardNumber = creditCardPayment.CreditCardNumber,
            CreditCardExpirationDate = creditCardPayment.CreditCardExpirationDate,
            CreditCardCvv = creditCardPayment.CreditCardCvv,
            PaymentId = creditCardPayment.Payment.Id
        };
    }
}