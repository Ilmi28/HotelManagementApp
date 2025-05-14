using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetRewardById;

public class GetLoyaltyRewardByIdQueryHandler(ILoyaltyRewardsRepository loyaltyRewardsRepository) : IRequestHandler<GetLoyaltyRewardByIdQuery, RewardResponse>
{
    public async Task<RewardResponse> Handle(GetLoyaltyRewardByIdQuery request, CancellationToken cancellationToken)
    {
        var reward = await loyaltyRewardsRepository.GetLoyaltyRewardById(request.LoyaltyRewardId, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty reward with id: {request.LoyaltyRewardId} not found.");
        
        return new RewardResponse
        {
            RewardId = reward.Id,
            RewardName = reward.RewardName,
            RewardDescription = reward.Description,
            Points = reward.PointsRequired
        };
    }
}