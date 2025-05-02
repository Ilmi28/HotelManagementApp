namespace HotelManagementApp.Application.Responses.HotelResponses;

public class HotelServiceResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int HotelId { get; set; }
}
