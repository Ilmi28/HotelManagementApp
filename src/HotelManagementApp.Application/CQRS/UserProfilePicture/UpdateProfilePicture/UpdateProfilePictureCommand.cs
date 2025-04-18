using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelManagementApp.Application.CQRS.UserProfilePicture.UpdateProfilePicture;

public class UpdateProfilePictureCommand : IRequest<string>
{
    public required string UserId { get; set; }
    public required IFormFile File { get; set; }
}
