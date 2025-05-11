using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetAllPayments;

public class GetAllPaymentsQuery : IRequest<ICollection<PaymentResponse>>
{
    
}