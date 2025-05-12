using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Update;

public class UpdateReviewCommand : IRequest
{
    public required int ReviewId { get; set; }
    [MaxLength(50)]
    public string? UserName { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    [MaxLength(500)]
    public required string? Review { get; set; }
}