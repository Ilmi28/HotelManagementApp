using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveServiceDiscount;

public class RemoveServiceDiscountCommand : IRequest
{
    public required int DiscountId { get; set; }
}
