using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetRewardById;

public class GetLoyaltyRewardByIdQuery : IRequest<RewardResponse>
{
    public required int LoyaltyRewardId { get; set; }
}