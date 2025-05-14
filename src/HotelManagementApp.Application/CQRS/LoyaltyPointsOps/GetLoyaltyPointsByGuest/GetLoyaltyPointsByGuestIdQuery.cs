using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsByGuest;

public class GetLoyaltyPointsByGuestIdQuery : IRequest<LoyaltyPointsGuestResponse>
{
    public required string GuestId { get; set; }
}