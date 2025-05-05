namespace HotelManagementApp.Core.Models.LoyaltyPointsModels;

public class LoyaltyReward
{
    public int Id { get; set; }
    public required string RewardName { get; set; }
    public required int PointsRequired { get; set; }
    public required string Description { get; set; }
}
