namespace HotelManagementApp.Application.Responses.LoyaltyPointsResponses;

public class RewardResponse
{
    public required int RewardId { get; set; }
    public required string RewardName { get; set; }
    public required string RewardDescription { get; set; }
    public int Points { get; set; }
}