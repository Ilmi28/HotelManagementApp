using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentById;

public class GetPaymentByIdQuery : IRequest<PaymentResponse>
{
    public required int PaymentId { get; set; }
}