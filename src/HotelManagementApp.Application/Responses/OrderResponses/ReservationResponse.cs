using HotelManagementApp.Application.Responses.HotelResponses;

namespace HotelManagementApp.Application.Responses.OrderResponses;

public class ReservationResponse
{
    public int Id { get; set; }
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
    public required int RoomId { get; set; }
    public required string UserId { get; set; }
    public required int OrderId { get; set; }
}