using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.DiscountModels;

public class ParkingDiscount
{
    public int Id { get; set; }
    public required HotelParking Parking { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
