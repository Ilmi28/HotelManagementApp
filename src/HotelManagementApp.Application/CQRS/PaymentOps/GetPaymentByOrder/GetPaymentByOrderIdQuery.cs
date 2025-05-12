using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentByOrder;

public class GetPaymentByOrderIdQuery : IRequest<PaymentResponse>
{
    public required int OrderId { get; set; }
}