using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryByGuest;

public class GetLoyaltyPointsHistoryByGuestIdQueryHandler(ILoyaltyPointsHistoryRepository loyaltyPointsHistoryRepository) 
    : IRequestHandler<GetLoyaltyPointsHistoryByGuestIdQuery, ICollection<LoyaltyPointsHistoryLogResponse>>
{
    public async Task<ICollection<LoyaltyPointsHistoryLogResponse>> Handle(GetLoyaltyPointsHistoryByGuestIdQuery request, CancellationToken cancellationToken)
    {
        var history = await loyaltyPointsHistoryRepository.GetLoyaltyPointsHistoryByGuestId(request.GuestId, cancellationToken);
        var response = new List<LoyaltyPointsHistoryLogResponse>();
        foreach (var log in history)
        {
            response.Add(new LoyaltyPointsHistoryLogResponse
            {
                GuestId = log.UserId,
                Points = log.Points,
                Description = log.Description,
                Date = log.Date,
            });
        }
        return response;
    }
}