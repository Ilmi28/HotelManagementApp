using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsHistoryByGuest;

public class GetLoyaltyPointsHistoryByGuestIdQuery : IRequest<ICollection<LoyaltyPointsHistoryLogResponse>>
{
    public required string GuestId { get; set; }
}