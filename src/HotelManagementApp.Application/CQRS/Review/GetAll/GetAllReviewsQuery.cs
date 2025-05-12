using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetAll;

public class GetAllReviewsQuery : IRequest<ICollection<HotelReviewResponse>>
{
    
}