namespace HotelManagementApp.Core.Models.LoyaltyPointsModels;

public class LoyaltyRewardUser
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required LoyaltyReward LoyaltyReward { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}