using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.ExchangePointsForReward;

public class ExchangePointsForRewardCommand : IRequest
{
    public required string GuestId { get; set; }
    public required int RewardId { get; set; }
}