using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPaymentByPayment;

public class GetCashPaymentByPaymentIdQuery : IRequest<CashPaymentResponse>
{
    public required int PaymentId { get; set; }
}