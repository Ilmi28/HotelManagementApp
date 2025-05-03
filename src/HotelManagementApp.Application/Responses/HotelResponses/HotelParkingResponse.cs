using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Application.Responses.HotelResponses;

public class HotelParkingResponse
{
    public int Id { get; set; }
    public int CarSpaces { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public required int HotelId { get; set; }
}
