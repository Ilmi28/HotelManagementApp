using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetById;

public class GetReviewByIdQuery : IRequest<HotelReviewResponse>
{
    public required int ReviewId { get; set; }
}