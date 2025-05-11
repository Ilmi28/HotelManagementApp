using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetPaymentMethods;

public class GetPaymentMethodsQuery : IRequest<ICollection<PaymentMethodResponse>>
{
    
}