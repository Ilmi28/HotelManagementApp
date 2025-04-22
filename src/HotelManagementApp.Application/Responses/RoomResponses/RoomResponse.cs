namespace HotelManagementApp.Application.Responses.RoomResponses;

public class RoomResponse
{
    public int Id { get; set; }
    public required string RoomName { get; set; }
    public required string RoomType { get; set; }
    public required decimal Price { get; set; }
    public required int HotelId { get; set; }
}
