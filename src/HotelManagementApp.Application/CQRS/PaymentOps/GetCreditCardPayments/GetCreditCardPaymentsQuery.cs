using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPayments;

public class GetCreditCardPaymentsQuery : IRequest<ICollection<CreditCardPaymentResponse>>
{
    
}