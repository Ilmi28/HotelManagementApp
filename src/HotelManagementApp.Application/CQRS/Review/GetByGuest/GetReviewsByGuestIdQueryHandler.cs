using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetByGuest;

public class GetReviewsByGuestIdQueryHandler(
    IHotelReviewRepository reviewRepository,
    IHotelReviewImageRepository reviewImageRepository,
    IFileService fileService) : IRequestHandler<GetReviewsByGuestIdQuery, ICollection<HotelReviewResponse>>
{
    public async Task<ICollection<HotelReviewResponse>> Handle(GetReviewsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetReviewsByGuest(request.GuestId, cancellationToken);
        var response = new List<HotelReviewResponse>();
        
        foreach (var review in reviews)
        {
            var reviewImages = await reviewImageRepository.GetReviewImagesByReviewId(review.Hotel.Id, cancellationToken);
            
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