namespace HotelManagementApp.Core.Models.HotelModels;

public class City
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required string Country { get; set; }
}
