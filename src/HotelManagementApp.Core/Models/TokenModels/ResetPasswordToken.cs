namespace HotelManagementApp.Core.Models.TokenModels;

public class ResetPasswordToken
{
    public int Id { get; set; }
    public required string ResetPasswordTokenHash { get; set; }
    public required string UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsRevoked { get; set; } = false;
}
