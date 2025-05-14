using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryOfGuests;

public class GetLoyaltyPointsHistoryOfGuestsQueryHandler(ILoyaltyPointsHistoryRepository historyRepository) 
    : IRequestHandler<GetLoyaltyPointsHistoryOfGuestsQuery, ICollection<LoyaltyPointsHistoryLogResponse>>
{
    public async Task<ICollection<LoyaltyPointsHistoryLogResponse>> Handle(GetLoyaltyPointsHistoryOfGuestsQuery request, CancellationToken cancellationToken)
    {
        var history = await historyRepository.GetAllLoyaltyPointsHistory(cancellationToken);
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