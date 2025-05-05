using HotelManagementApp.Application.Responses.DiscountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByHotel;

public class GetDiscountByHotelQuery : IRequest<ICollection<HotelDiscountResponse>>
{
    public required int HotelId { get; set; }
}
