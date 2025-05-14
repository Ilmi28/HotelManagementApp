namespace HotelManagementApp.Core.Models.LoyaltyPointsModels;

public class LoyaltyPointsLog
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required int Points { get; set; }
    public required string Description { get; set; }
}
