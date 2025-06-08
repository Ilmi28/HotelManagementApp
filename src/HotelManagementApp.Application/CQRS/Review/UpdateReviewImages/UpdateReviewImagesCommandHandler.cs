using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.UpdateReviewImages;

public class UpdateReviewImagesCommandHandler(
    IHotelReviewRepository reviewRepository,
    IHotelReviewImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<UpdateReviewImagesCommand>
{
    public async Task Handle(UpdateReviewImagesCommand request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewById(request.ReviewId, cancellationToken)
            ?? throw new ReviewNotFoundException($"Review with id {request.ReviewId} not found");
        var reviewImages = await imageRepository.GetReviewImagesByReviewId(request.ReviewId, cancellationToken);
        foreach (var reviewImage in reviewImages)
            fileService.DeleteFile("images", reviewImage.FileName);
        await imageRepository.RemoveReviewImagesByReviewId(review.Id, cancellationToken);
        foreach (var file in request.ReviewImages)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            var imageName = fileService.UploadFile("images", stream.ToArray(), Path.GetExtension(file.FileName));
            await imageRepository.AddReviewImage(new HotelReviewImage
            {
                FileName = imageName,
                HotelReview = review
            }, cancellationToken);
        }
        review.LastModified = DateTime.Now;
    }
}