using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Application.Responses.DiscountResponses;

public class ParkingDiscountResponse
{
    public int Id { get; set; }
    public required int ParkingId { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
