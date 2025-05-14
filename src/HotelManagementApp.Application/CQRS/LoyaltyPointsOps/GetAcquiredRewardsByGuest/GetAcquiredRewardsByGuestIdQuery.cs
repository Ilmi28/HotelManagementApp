using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetAcquiredRewardsByGuest;

public class GetAcquiredRewardsByGuestIdQuery : IRequest<ICollection<LoyaltyRewardGuestResponse>>
{
    public required string GuestId { get; set; }
}