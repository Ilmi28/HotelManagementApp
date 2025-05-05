using HotelManagementApp.Application.Responses.DiscountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByParking;

public class GetDiscountsByParkingQuery : IRequest<ICollection<ParkingDiscountResponse>>
{
    public required int ParkingId { get; set; }
}
