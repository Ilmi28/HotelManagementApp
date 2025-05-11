using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPayments;

public class GetCashPaymentsQuery : IRequest<ICollection<CashPaymentResponse>>
{
    
}