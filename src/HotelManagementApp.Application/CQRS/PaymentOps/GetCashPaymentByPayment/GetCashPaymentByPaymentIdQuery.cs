using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPaymentByPayment;

public class GetCashPaymentByPaymentIdQuery : IRequest<CashPaymentResponse>
{
    public required int PaymentId { get; set; }
}