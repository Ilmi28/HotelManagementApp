namespace HotelManagementApp.Core.Models.LoyaltyPointsModels;

public class LoyaltyPointsLog
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required DateTime Date { get; set; }
    public required int Points { get; set; }
}
