using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Add;

public class AddReviewCommand : IRequest
{
    [MaxLength(50)]
    public string? UserName { get; set; }
    [Required]
    public required string UserId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    [MaxLength(500)]
    public required string? Review { get; set; }
    [Required]
    public required int HotelId { get; set; }
}