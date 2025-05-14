using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAvailableRewards;

public class GetAvailableRewardsQueryHandler(ILoyaltyRewardsRepository rewardRepository) 
    : IRequestHandler<GetAvailableRewardsQuery, ICollection<RewardResponse>>
{
    public async Task<ICollection<RewardResponse>> Handle(GetAvailableRewardsQuery request, CancellationToken cancellationToken)
    {
        var rewards = await rewardRepository.GetAllLoyaltyRewards(cancellationToken);
        
        return rewards.Select(r => new RewardResponse
        {
            RewardId = r.Id,
            RewardName = r.RewardName,
            RewardDescription = r.Description,
            Points = r.PointsRequired
        }).ToList();
    }
}