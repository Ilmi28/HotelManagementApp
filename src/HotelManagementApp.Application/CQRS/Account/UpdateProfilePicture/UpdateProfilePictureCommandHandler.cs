using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.AccountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;

public class UpdateProfilePictureCommandHandler(
    IFileService fileService,
    IProfilePictureRepository profilePictureRepository) : IRequestHandler<UpdateProfilePictureCommand, string>
{
    public async Task<string> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {   
        var prevProfilePicture = await profilePictureRepository.GetProfilePicture(request.UserId, cancellationToken);
        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream);
        var imageName = fileService.UploadFile("images", stream.ToArray(), Path.GetExtension(request.File.FileName));
        if (prevProfilePicture != null)
        {
            fileService.DeleteFile("images", prevProfilePicture.FileName);
            await profilePictureRepository.RemoveProfilePicture(request.UserId, cancellationToken);
        }
        var profilePicture = new ProfilePicture
        {
            UserId = request.UserId,
            FileName = imageName
        };
        await profilePictureRepository.AddProfilePicture(profilePicture, cancellationToken);
        return fileService.GetFileUrl("images", imageName);
    }
}
