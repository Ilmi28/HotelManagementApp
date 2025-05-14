using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.UpdateLoyaltyReward;

public class UpdateLoyaltyRewardCommandHandler(ILoyaltyRewardsRepository rewardsRepository) : IRequestHandler<UpdateLoyaltyRewardCommand>
{
    public async Task Handle(UpdateLoyaltyRewardCommand request, CancellationToken cancellationToken)
    {
        var reward = await rewardsRepository.GetLoyaltyRewardById(request.LoyaltyRewardId, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty reward with id: {request.LoyaltyRewardId} not found.");
        reward.RewardName = request.RewardName;
        reward.Description = request.Description;
        reward.PointsRequired = request.PointsRequired;
        await rewardsRepository.UpdateLoyaltyReward(reward, cancellationToken);
    }
}