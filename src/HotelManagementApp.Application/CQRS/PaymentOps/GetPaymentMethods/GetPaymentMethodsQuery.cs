using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentMethods;

public class GetPaymentMethodsQuery : IRequest<ICollection<PaymentMethodResponse>>
{
    
}