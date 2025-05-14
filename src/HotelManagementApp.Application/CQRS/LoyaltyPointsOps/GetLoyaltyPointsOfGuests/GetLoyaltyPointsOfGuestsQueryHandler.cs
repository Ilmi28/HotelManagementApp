using HotelManagementApp.Application.Responses.LoyaltyPointsResponses;
using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.LoyaltyPointsOps.GetLoyaltyPointsOfGuests;

public class GetLoyaltyPointsOfGuestsQueryHandler(ILoyaltyPointsRepository pointsRepository) : IRequestHandler<GetLoyaltyPointsOfGuestsQuery, ICollection<LoyaltyPointsGuestResponse>>
{
    public async Task<ICollection<LoyaltyPointsGuestResponse>> Handle(GetLoyaltyPointsOfGuestsQuery request, CancellationToken cancellationToken)
    {
        var points = await pointsRepository.GetAllLoyaltyPoints(cancellationToken);
        var response = new List<LoyaltyPointsGuestResponse>();
        foreach (var point in points)
        {
            response.Add(new LoyaltyPointsGuestResponse
            {
                GuestId = point.GuestId,
                Points = point.Points,
            });
        }
        return response;
    }
}