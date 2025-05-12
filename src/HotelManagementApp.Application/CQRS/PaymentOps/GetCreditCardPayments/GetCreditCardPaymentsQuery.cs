using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPayments;

public class GetCreditCardPaymentsQuery : IRequest<ICollection<CreditCardPaymentResponse>>
{
    
}