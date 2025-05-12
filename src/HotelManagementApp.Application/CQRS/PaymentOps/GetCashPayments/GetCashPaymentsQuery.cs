using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetCashPayments;

public class GetCashPaymentsQuery : IRequest<ICollection<CashPaymentResponse>>
{
    
}