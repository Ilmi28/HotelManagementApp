using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveHotelDiscount;

public class RemoveHotelDiscountCommand : IRequest
{
    public required int DiscountId { get; set; }
}
