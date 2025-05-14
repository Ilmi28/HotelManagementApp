using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAcquiredRewardsByGuest;

public class GetAcquiredRewardsByGuestIdQueryHandler(
    ILoyaltyRewardUserRepository rewardUserRepository, 
    IUserManager userManager) : IRequestHandler<GetAcquiredRewardsByGuestIdQuery, ICollection<LoyaltyRewardGuestResponse>>
{
    public async Task<ICollection<LoyaltyRewardGuestResponse>> Handle(GetAcquiredRewardsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.GuestId)
            ?? throw new UnauthorizedAccessException();
        var userRewards = await rewardUserRepository.GetLoyaltyRewardsByUserId(user.Id, cancellationToken);
        var response = new List<LoyaltyRewardGuestResponse>();
        foreach (var reward in userRewards)
        {
            response.Add(new LoyaltyRewardGuestResponse
            {
                GuestId = user.Id,
                RewardId = reward.LoyaltyReward.Id,
                Date = reward.Date,
            });
        }
        return response;
    }
}