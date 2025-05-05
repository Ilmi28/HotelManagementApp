using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Application.Responses.DiscountResponses;

public class ServiceDiscountResponse
{
    public int Id { get; set; }
    public required int ServiceId { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
