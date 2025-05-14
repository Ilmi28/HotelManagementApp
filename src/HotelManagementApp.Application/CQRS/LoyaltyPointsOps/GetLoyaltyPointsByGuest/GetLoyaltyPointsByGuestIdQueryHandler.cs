using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsByGuest;

public class GetLoyaltyPointsByGuestIdQueryHandler(
    ILoyaltyPointsRepository loyaltyPointsRepository, IUserManager userManager) : IRequestHandler<GetLoyaltyPointsByGuestIdQuery, LoyaltyPointsGuestResponse>
{
    public async Task<LoyaltyPointsGuestResponse> Handle(GetLoyaltyPointsByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.GuestId)
                   ?? throw new UnauthorizedAccessException();
        var points = await loyaltyPointsRepository.GetLoyaltyPointsByGuestId(user.Id, cancellationToken)
            ?? throw new LoyaltyRewardNotFoundException($"Loyalty points for guest with id: {request.GuestId} not found.");
        return new LoyaltyPointsGuestResponse
        {
            GuestId = user.Id,
            Points = points.Points
        };
    }
}