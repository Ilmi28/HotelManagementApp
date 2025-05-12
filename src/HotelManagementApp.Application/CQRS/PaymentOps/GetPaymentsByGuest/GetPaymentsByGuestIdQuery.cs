using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentsByGuest;

public class GetPaymentsByGuestIdQuery : IRequest<ICollection<PaymentResponse>>
{
    public required string GuestId { get; set; }
}