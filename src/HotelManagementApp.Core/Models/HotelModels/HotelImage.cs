namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelImage
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required Hotel Hotel { get; set; }
}
