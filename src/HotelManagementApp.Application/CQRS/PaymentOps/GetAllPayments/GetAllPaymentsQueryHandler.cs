using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetAllPayments;

public class GetAllPaymentsQueryHandler(IPaymentRepository paymentRepository) 
    : IRequestHandler<GetAllPaymentsQuery, ICollection<PaymentResponse>>
{
    public async Task<ICollection<PaymentResponse>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await paymentRepository.GetPayments(cancellationToken);
        var response = new List<PaymentResponse>();
        
        foreach (var payment in payments)
        {
            response.Add(new PaymentResponse
            {
                Id = payment.Id,
                PaymentMethod = payment.PaymentMethod.ToString().ToUpper(),
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Date = payment.Date
            });
        }
        
        return response;
    }
}