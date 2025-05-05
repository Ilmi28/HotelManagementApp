using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.DiscountModels;

public class RoomDiscount
{
    public int Id { get; set; }
    public required HotelRoom Room { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
