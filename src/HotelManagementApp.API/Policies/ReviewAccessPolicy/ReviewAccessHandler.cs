using System.Security.Claims;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Policies.ReviewAccessPolicy;

public class ReviewAccessHandler(IHotelReviewRepository reviewRepository) : AuthorizationHandler<ReviewAccessRequirement, int>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReviewAccessRequirement requirement, int reviewId)
    {
        var review = await reviewRepository.GetReviewById(reviewId, CancellationToken.None);
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (review != null && review.UserId == userId)
            context.Succeed(requirement);
    }
}