using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentById;

public class GetPaymentByIdQuery : IRequest<PaymentResponse>
{
    public required int PaymentId { get; set; }
}