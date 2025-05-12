using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetByHotel;

public class GetReviewsByHotelIdQueryHandler(
    IHotelReviewRepository reviewRepository,
    IHotelReviewImageRepository reviewImageRepository,
    IFileService fileService) : IRequestHandler<GetReviewsByHotelIdQuery, ICollection<HotelReviewResponse>>
{
    public async Task<ICollection<HotelReviewResponse>> Handle(GetReviewsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetReviewsByHotel(request.HotelId, cancellationToken);
        var response = new List<HotelReviewResponse>();
        
        foreach (var review in reviews)
        {
            var hotelImages = await reviewImageRepository.GetReviewImagesByReviewId(review.Hotel.Id, cancellationToken);
            
            response.Add(new HotelReviewResponse
            {
                Id = review.Id,
                HotelId = review.Hotel.Id,
                UserName = review.UserName,
                Rating = review.Rating,
                Review = review.Review,
                Created = review.Created,
                LastModified = review.LastModified,
                ReviewImages = hotelImages.Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
            });
        }
    
        return response;
    }
    
}