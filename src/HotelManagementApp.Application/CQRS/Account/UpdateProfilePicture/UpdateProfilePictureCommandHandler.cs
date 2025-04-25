using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;

public class UpdateProfilePictureCommandHandler(
    IImageService imageService,
    IProfilePictureRepository profilePictureRepository) : IRequestHandler<UpdateProfilePictureCommand, string>
{
    public async Task<string> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {   
        var prevProfilePicture = await profilePictureRepository.GetProfilePicture(request.UserId, cancellationToken);
        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream);
        var imageName = imageService.UploadImage(stream.ToArray());
        if (prevProfilePicture != null)
        {
            imageService.DeleteImage(prevProfilePicture.FileName);
            await profilePictureRepository.RemoveProfilePicture(request.UserId, cancellationToken);
        }
        var profilePicture = new ProfilePicture
        {
            UserId = request.UserId,
            FileName = imageName
        };
        await profilePictureRepository.AddProfilePicture(profilePicture, cancellationToken);
        return imageName;
    }
}
