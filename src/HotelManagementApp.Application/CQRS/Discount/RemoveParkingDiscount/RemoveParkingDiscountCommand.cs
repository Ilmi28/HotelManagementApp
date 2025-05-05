using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveParkingDiscount;

public class RemoveParkingDiscountCommand : IRequest
{
    public required int DiscountId { get; set; }
}
