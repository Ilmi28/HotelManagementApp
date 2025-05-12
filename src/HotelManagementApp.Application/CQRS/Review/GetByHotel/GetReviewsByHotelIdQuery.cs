using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetByHotel;

public class GetReviewsByHotelIdQuery : IRequest<ICollection<HotelReviewResponse>>
{
    public required int HotelId { get; set; }
}