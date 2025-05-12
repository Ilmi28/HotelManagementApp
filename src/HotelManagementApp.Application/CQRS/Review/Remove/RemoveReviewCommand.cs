using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Remove;

public class RemoveReviewCommand : IRequest
{
    public required int ReviewId { get; set; }
}