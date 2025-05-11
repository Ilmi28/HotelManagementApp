using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetPaymentMethods;

public class GetPaymentMethodsQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentMethodsQuery, ICollection<PaymentMethodResponse>>
{
    public async Task<ICollection<PaymentMethodResponse>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var paymentMethods = await paymentRepository.GetPaymentMethods(cancellationToken);
        return paymentMethods.Select(pm => new PaymentMethodResponse
        {
            Id = pm.Id,
            Name = pm.Name.ToString()
        }).ToList();
    }
}