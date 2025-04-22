namespace HotelManagementApp.Application.Responses.HotelResponses;

public class HotelResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
}
