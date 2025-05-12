using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelManagementApp.Application.CQRS.Review.UpdateReviewImages;

public class UpdateReviewImagesCommand : IRequest
{
    public int ReviewId { get; set; }
    public List<IFormFile> ReviewImages { get; set; } = new List<IFormFile>();
}