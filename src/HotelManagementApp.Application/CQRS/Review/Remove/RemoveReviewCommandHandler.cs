using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Remove;

public class RemoveReviewCommandHandler(
    IHotelReviewRepository reviewRepository,
    IHotelReviewImageRepository hotelReviewImageRepository) : IRequestHandler<RemoveReviewCommand>
{
    public async Task Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewById(request.ReviewId, cancellationToken)
            ?? throw new ReviewNotFoundException($"Review with id {request.ReviewId} not found");
        await reviewRepository.RemoveReview(review, cancellationToken);
        await hotelReviewImageRepository.RemoveReviewImagesByReviewId(review.Id, cancellationToken);
    }
}