using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.PayWithCash;

public class PayWithCashCommand : IRequest
{
    public required int OrderId { get; set; }
}