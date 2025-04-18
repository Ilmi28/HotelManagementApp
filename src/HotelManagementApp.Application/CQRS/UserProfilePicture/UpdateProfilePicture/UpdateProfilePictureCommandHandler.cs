using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using HotelManagementApp.Core.Exceptions.BadRequest;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Application.CQRS.UserProfilePicture.UpdateProfilePicture;

public class UpdateProfilePictureCommandHandler(
    IImageService imageService,
    IProfilePictureRepository profilePictureRepository,
    IUserManager userManager,
    IAuthenticationService authenticationService,
    IAuthorizationService authorizationService,
    IConfiguration config) : IRequestHandler<UpdateProfilePictureCommand, string>
{
    public async Task<string> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {   
        
        var isValidExtension = IsValidExtension(request.File.FileName);
        if (!isValidExtension)
            throw new InvalidImageTypeException("Invalid file extension.");
        var isAuthorized = await IsAuthorized(request.UserId);
        if (!isAuthorized)
            throw new UnauthorizedAccessException();
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

    private async Task<bool> IsAuthorized(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;
        var loggedInUser = authenticationService.GetLoggedInUser();
        if (loggedInUser == null)
            return false;
        var ownerPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new AccountOwnerRequirement());
        var hierarchyPolicy = await authorizationService.AuthorizeAsync(loggedInUser, user, new RoleHierarchyRequirement());
        if (ownerPolicy.Succeeded || hierarchyPolicy.Succeeded)
            return true;
        return false;
    }

    private bool IsValidExtension(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName).ToLower();
        var allowedExtensions = config.GetSection("AllowedImageExtensions").Get<string[]>();
        if (allowedExtensions == null)
            allowedExtensions = Array.Empty<string>();
        return allowedExtensions.Contains(fileExtension);
    }
}
