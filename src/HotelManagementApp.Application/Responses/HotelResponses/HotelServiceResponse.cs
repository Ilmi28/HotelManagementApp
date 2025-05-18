namespace HotelManagementApp.Application.Responses.HotelResponses;

public class HotelServiceResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public decimal Discount { get; set; }
    public required decimal FinalPrice { get; set; }
    public required int HotelId { get; set; }
}
