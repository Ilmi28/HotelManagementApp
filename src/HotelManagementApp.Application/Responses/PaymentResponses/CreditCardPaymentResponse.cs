namespace HotelManagementApp.Application.Responses.PaymentResponses;

public class CreditCardPaymentResponse
{
    public int Id { get; set; }
    public required string CreditCardNumber { get; set; }
    public required string CreditCardExpirationDate { get; set; }
    public required string CreditCardCvv { get; set; }
    public required int PaymentId { get; set; }
}