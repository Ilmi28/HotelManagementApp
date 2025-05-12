using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPaymentByPayment;

public class GetCreditCardPaymentByPaymentIdQuery : IRequest<CreditCardPaymentResponse>
{
    public required int PaymentId { get; set; }
}