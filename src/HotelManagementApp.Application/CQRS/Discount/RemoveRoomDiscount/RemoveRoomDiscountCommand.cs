using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveRoomDiscount;

public class RemoveRoomDiscountCommand : IRequest
{
    public required int DiscountId { get; set; }
}
