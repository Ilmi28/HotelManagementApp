using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetAllPayments;

public class GetAllPaymentsQuery : IRequest<ICollection<PaymentResponse>>
{
    
}