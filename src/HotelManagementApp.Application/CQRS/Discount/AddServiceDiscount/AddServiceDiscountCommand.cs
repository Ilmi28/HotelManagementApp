using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Discount.AddServiceDiscount;

public class AddServiceDiscountCommand : IRequest
{
    public required int ServiceId { get; set; }
    [Required]
    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100.")]
    public required int DiscountPercent { get; set; }
    [Required]
    public required DateTime From { get; set; }
    [Required]
    public required DateTime To { get; set; }
}
