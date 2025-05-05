namespace HotelManagementApp.Core.Models.DiscountModels;

public class DiscountCode
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required int DiscountAmount { get; set; }
    public required DateTime ExpirationDate { get; set; }
}
