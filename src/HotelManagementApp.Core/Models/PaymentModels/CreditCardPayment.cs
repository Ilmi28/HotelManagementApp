using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Models.PaymentModels;

public class CreditCardPayment
{
    public int Id { get; set; }
    public required string CreditCardNumber { get; set; }   
    public required string CreditCardExpirationDate { get; set; }
    public required string CreditCardCvv { get; set; }
    public required Payment Payment { get; set; }
}