using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.UpdateRoomImages;

public class UpdateRoomImagesCommandHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository, 
    IFileService fileService) : IRequestHandler<UpdateRoomImagesCommand>
{
    public async Task Handle(UpdateRoomImagesCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var roomImages = await imageRepository.GetRoomImagesByRoomId(request.RoomId, cancellationToken);
        foreach (var roomImage in roomImages)
            fileService.DeleteFile("images", roomImage.FileName);
        foreach (var file in request.RoomImages)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            var imageName = fileService.UploadFile("images", stream.ToArray(), Path.GetExtension(file.FileName));
            await imageRepository.AddRoomImage(new HotelRoomImage
            {
                FileName = imageName,
                Room = room
            }, cancellationToken);
        }
        await imageRepository.RemoveRoomImagesByRoomId(room.Id, cancellationToken);

    }
}
