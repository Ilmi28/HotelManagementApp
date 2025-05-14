using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsOfGuests;

public class GetLoyaltyPointsOfGuestsQuery : IRequest<ICollection<LoyaltyPointsGuestResponse>>
{
    
}