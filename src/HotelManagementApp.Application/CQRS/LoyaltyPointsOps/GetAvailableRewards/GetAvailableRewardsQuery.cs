using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAvailableRewards;

public class GetAvailableRewardsQuery : IRequest<ICollection<RewardResponse>>
{
    
}