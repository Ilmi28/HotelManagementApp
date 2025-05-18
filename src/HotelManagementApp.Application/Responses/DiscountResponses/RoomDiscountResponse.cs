namespace HotelManagementApp.Application.Responses.DiscountResponses;

public class RoomDiscountResponse
{
    public int Id { get; set; }
    public required int RoomId { get; set; }
    public required DateTime From { get; set; }
    public required DateTime To { get; set; }
    public required int DiscountPercent { get; set; }
}
