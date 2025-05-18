using HotelManagementApp.Application.Responses.PaymentResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentsByGuest;

public class GetPaymentsByGuestIdQueryHandler(
    IUserManager userManager,
    IPaymentRepository paymentRepository) 
    : IRequestHandler<GetPaymentsByGuestIdQuery, ICollection<PaymentResponse>>
{
    public async Task<ICollection<PaymentResponse>> Handle(GetPaymentsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.GuestId)
                   ?? throw new UserNotFoundException($"User with id {request.GuestId} not found");
        var payments = await paymentRepository.GetPaymentsByUserId(request.GuestId, cancellationToken);
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