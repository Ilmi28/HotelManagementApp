namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelService
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required HotelModel Hotel { get; set; }
}
