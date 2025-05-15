using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.ExchangePointsForReward;

public class ExchangePointsForRewardCommandHandler(
    IUserManager userManager, 
    ILoyaltyRewardsRepository rewardsRepository,
    ILoyaltyPointsRepository loyaltyPointsRepository,
    ILoyaltyRewardUserRepository rewardUserRepository) : IRequestHandler<ExchangePointsForRewardCommand>
{
    public async Task Handle(ExchangePointsForRewardCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.GuestId)
            ?? throw new UnauthorizedAccessException();
        if (!user.Roles.Contains("Guest"))
            throw new UnauthorizedAccessException();
        var reward = await rewardsRepository.GetLoyaltyRewardById(request.RewardId, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty reward with id: {request.RewardId} not found.");
        var points = await loyaltyPointsRepository.GetLoyaltyPointsByGuestId(user.Id, cancellationToken);
        if (points is null || points.Points < reward.PointsRequired)
            throw new InvalidOperationException($"Not enough points for user with id: {user.Id} to get reward with id: {reward.Id}");
        points.Points -= reward.PointsRequired;
        await loyaltyPointsRepository.UpdateLoyaltyPoints(points, cancellationToken);
        await rewardUserRepository.AddLoyaltyRewardUser(new LoyaltyRewardUser
        {
            UserId = user.Id,
            LoyaltyReward = reward
        }, cancellationToken);
    }
}