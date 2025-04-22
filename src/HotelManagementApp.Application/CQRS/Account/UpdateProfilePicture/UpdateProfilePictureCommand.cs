using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;

public class UpdateProfilePictureCommand : IRequest<string>
{
    public required string UserId { get; set; }
    [FileExtensions(Extensions = ".jpg,.jpeg,.png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png).")]
    public required IFormFile File { get; set; }
}
