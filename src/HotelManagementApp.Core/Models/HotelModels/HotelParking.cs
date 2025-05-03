namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelParking
{
    public int Id { get; set; }
    public int CarSpaces { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public required Hotel Hotel { get; set; }
}
