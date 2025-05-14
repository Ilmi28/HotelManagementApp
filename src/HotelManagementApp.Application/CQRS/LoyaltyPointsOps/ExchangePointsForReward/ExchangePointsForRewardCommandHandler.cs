using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.ExchangePointsForReward;

public class ExchangePointsForRewardCommandHandler(
    IUserManager userManager, 
    ILoyaltyRewardsRepository rewardsRepository,
    ILoyaltyPointsRepository loyaltyPointsRepository) : IRequestHandler<ExchangePointsForRewardCommand>
{
    public async Task Handle(ExchangePointsForRewardCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.GuestId)
            ?? throw new UnauthorizedAccessException();
        if (!user.Roles.Contains("Guest"))
            throw new UnauthorizedAccessException();
        var reward = await rewardsRepository.GetLoyaltyRewardById(request.RewardId, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty reward with id: {request.RewardId} not found.");
        var points = await loyaltyPointsRepository.GetLoyaltyPointsByGuestId(user.Id, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty points for guest with id: {request.GuestId} not found.");
        if (points.Points < reward.PointsRequired)
            throw new InvalidOperationException("Not enough points");
        points.Points -= reward.PointsRequired;
        await loyaltyPointsRepository.UpdateLoyaltyPoints(points, cancellationToken);

    }
}