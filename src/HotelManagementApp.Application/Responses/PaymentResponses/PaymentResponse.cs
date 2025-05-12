namespace HotelManagementApp.Application.Responses.PaymentResponses;

public class PaymentResponse
{
    public required int Id { get; set; }
    public required int OrderId { get; set; }
    public required string PaymentMethod { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime Date { get; set; }
}