using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.RemoveLoyaltyReward;

public class RemoveLoyaltyRewardCommandHandler(ILoyaltyRewardsRepository rewardsRepository) : IRequestHandler<RemoveLoyaltyRewardCommand>
{
    public async Task Handle(RemoveLoyaltyRewardCommand request, CancellationToken cancellationToken)
    {
        var reward = await rewardsRepository.GetLoyaltyRewardById(request.LoyaltyRewardId, cancellationToken)
                     ?? throw new LoyaltyRewardNotFoundException($"Loyalty reward with id: {request.LoyaltyRewardId} not found.");
        await rewardsRepository.RemoveLoyaltyReward(reward, cancellationToken);
    }
}