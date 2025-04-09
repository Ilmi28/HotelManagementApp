using Microsoft.AspNetCore.Http;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;

public class UpdateProfilePictureCommand
{
    public required string UserId { get; set; }
    public required IFormFile ProfilePicture { get; set; }
}
