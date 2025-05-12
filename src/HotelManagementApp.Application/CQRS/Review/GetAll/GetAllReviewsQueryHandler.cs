using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetAll;

public class GetAllReviewsQueryHandler(
    IHotelReviewRepository reviewRepository, 
    IHotelImageRepository hotelImageRepository,
    IFileService fileService) : IRequestHandler<GetAllReviewsQuery, ICollection<HotelReviewResponse>>
{
    public async Task<ICollection<HotelReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetAllReviews(cancellationToken);
        var response = new List<HotelReviewResponse>();
        
        foreach (var review in reviews)
        {
            var reviewImages = await hotelImageRepository.GetHotelImagesByHotelId(review.Hotel.Id, cancellationToken);
            
            response.Add(new HotelReviewResponse
            {
                Id = review.Id,
                HotelId = review.Hotel.Id,
                UserName = review.UserName,
                Rating = review.Rating,
                Review = review.Review,
                Created = review.Created,
                LastModified = review.LastModified,
                ReviewImages = reviewImages.Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
            });
        }
    
        return response;
    }
}