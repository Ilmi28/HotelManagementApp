namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelParking
{
    public int Id { get; set; }
    public int CarSpaces { get; set; }
    public required HotelModel Hotel { get; set; }
}
