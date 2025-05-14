using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.RemoveLoyaltyReward;

public class RemoveLoyaltyRewardCommand : IRequest
{
    public required int LoyaltyRewardId { get; set; }
}