using HotelManagementApp.Application.Responses.DiscountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByService;

public class GetDiscountsByServiceQuery : IRequest<ICollection<ServiceDiscountResponse>>
{
    public required int ServiceId { get; set; }
}
