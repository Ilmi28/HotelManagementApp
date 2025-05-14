using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryOfGuests;

public class GetLoyaltyPointsHistoryOfGuestsQuery : IRequest<ICollection<LoyaltyPointsHistoryLogResponse>>
{
    
}