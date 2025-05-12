using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.GetById;

public class GetReviewByidQueryHandler(
    IHotelReviewRepository reviewRepository,
    IHotelReviewImageRepository reviewImageRepository,
    IFileService fileService) : IRequestHandler<GetReviewByIdQuery, HotelReviewResponse>
{
    public async Task<HotelReviewResponse> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewById(request.ReviewId, cancellationToken)
            ?? throw new ReviewNotFoundException($"Review with id {request.ReviewId} not found");
            
        var reviewImages = await reviewImageRepository.GetReviewImagesByReviewId(review.Id, cancellationToken);
        
        return new HotelReviewResponse
        {
            Id = review.Id,
            HotelId = review.Hotel.Id,
            UserName = review.UserName,
            Rating = review.Rating,
            Review = review.Review,
            Created = review.Created,
            LastModified = review.LastModified,
            ReviewImages = reviewImages.Select(i => fileService.GetFileUrl("images", i.FileName)).ToList()
        };
    }
}