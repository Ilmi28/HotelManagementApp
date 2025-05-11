using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.PaymentOps.PayWithCreditCard;

public class PayWithCreditCardCommand : IRequest
{
    [Required]
    public required int OrderId { get; set; }
    [Required]
    [CreditCard]
    public required string CreditCardNumber { get; set; }
    [Required]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
    public required string CreditCardExpirationDate { get; set; }
    [Required]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiration date must be in MM/YY format.")]
    public required string CreditCardCvv { get; set; }
}