using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Update;

public class UpdateReviewCommandHandler(
    IUserManager userManager,
    IHotelReviewRepository reviewRepository) : IRequestHandler<UpdateReviewCommand>
{
    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewById(request.ReviewId, cancellationToken)
            ?? throw new ReviewNotFoundException($"Review with id {request.ReviewId} not found");
        
        var user = await userManager.FindByIdAsync(review.UserId)
                   ?? throw new UnauthorizedAccessException();
        if (review.UserId != user.Id)
            throw new UnauthorizedAccessException("You can only update your own reviews");

        review.Review = request.Review;
        review.Rating = request.Rating;
        review.UserName = request.UserName;
        review.LastModified = DateTime.Now;

        await reviewRepository.UpdateReview(review, cancellationToken);
    }
}