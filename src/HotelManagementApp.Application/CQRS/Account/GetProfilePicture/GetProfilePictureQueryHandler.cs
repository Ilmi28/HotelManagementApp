using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetProfilePicture;

public class GetProfilePictureQueryHandler(
    IImageService imageService, 
    IProfilePictureRepository profilePictureRepository,
    IUserManager userManager) : IRequestHandler<GetProfilePictureQuery, byte[]>
{
    public async Task<byte[]> Handle(GetProfilePictureQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException();
        var profilePicture = await profilePictureRepository.GetProfilePicture(request.UserId, cancellationToken);
        if (profilePicture == null)
        {
            var defaultProfilePicture = imageService.GetImage("defaultprofile.jpg");
            return defaultProfilePicture;
        }
        return imageService.GetImage(profilePicture.FileName);
    }
}
