namespace HotelManagementApp.Application.Responses.LoyaltyPointsResponses;

public class LoyaltyRewardGuestResponse
{
    public required string GuestId { get; set; }
    public required int RewardId { get; set; }
    public required DateTime Date { get; set; }
}