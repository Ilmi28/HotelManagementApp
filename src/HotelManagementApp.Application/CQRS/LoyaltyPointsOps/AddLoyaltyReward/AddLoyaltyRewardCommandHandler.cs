using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.AddLoyaltyReward;

public class AddLoyaltyRewardCommandHandler(ILoyaltyRewardsRepository rewardsRepository) : IRequestHandler<AddLoyaltyRewardCommand>
{
    public async Task Handle(AddLoyaltyRewardCommand request, CancellationToken cancellationToken)
    {
        var reward = new LoyaltyReward
        {
            RewardName = request.RewardName,
            Description = request.Description,
            PointsRequired = request.PointsRequired
        };
        await rewardsRepository.AddLoyaltyReward(reward, cancellationToken);
    }
}