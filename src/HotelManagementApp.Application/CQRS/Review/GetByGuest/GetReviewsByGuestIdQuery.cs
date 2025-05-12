using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetByGuest;

public class GetReviewsByGuestIdQuery : IRequest<ICollection<HotelReviewResponse>>
{
    public required string GuestId { get; set; }
}