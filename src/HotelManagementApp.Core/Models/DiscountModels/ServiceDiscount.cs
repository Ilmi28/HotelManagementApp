using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.DiscountModels;

public class ServiceDiscount
{
    public int Id { get; set; }
    public required HotelService Service { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
