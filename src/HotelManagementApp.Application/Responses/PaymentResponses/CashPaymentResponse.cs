namespace HotelManagementApp.Application.Responses.PaymentResponses;

public class CashPaymentResponse
{
    public int Id { get; set; }
    public required int PaymentId { get; set; }
}