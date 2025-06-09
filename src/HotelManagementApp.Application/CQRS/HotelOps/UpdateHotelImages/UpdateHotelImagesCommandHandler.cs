using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.UpdateHotelImages;

public class UpdateHotelImagesCommandHandler(
    IHotelRepository hotelRepository, 
    IHotelImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<UpdateHotelImagesCommand>
{
    public async Task Handle(UpdateHotelImagesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
                    ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelImages = await imageRepository.GetHotelImagesByHotelId(request.HotelId, cancellationToken);
        foreach (var hotelImage in hotelImages)
            fileService.DeleteFile("images", hotelImage.FileName);
        await imageRepository.RemoveHotelImagesByHotelId(hotel.Id, cancellationToken);
        foreach (var file in request.HotelImages)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var imageName = fileService.UploadFile("images", stream.ToArray(), Path.GetExtension(file.FileName));
            await imageRepository.AddHotelImage(new HotelImage
            {
                FileName = imageName,
                Hotel = hotel
            }, cancellationToken);
        }
    }
}