using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPaymentByPayment;

public class GetCreditCardPaymentByPaymentIdQuery : IRequest<CreditCardPaymentResponse>
{
    public required int PaymentId { get; set; }
}