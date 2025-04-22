namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelImage
{
    public required string Id { get; set; }
    public required string FileName { get; set; }
    public required HotelModel Hotel { get; set; }
}
