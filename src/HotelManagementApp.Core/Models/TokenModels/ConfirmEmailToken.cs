namespace HotelManagementApp.Core.Models.TokenModels;

public class ConfirmEmailToken
{
    public int Id { get; set; }
    public required string ConfirmEmailTokenHash { get; set; }
    public required string UserId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsRevoked { get; set; } = false;
}
