using HotelManagementApp.Application.ValidationAttributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture;

public class UpdateProfilePictureCommand : IRequest<string>
{
    public required string UserId { get; set; }
    [ValidFileExtension("jpg,jpeg,png", ErrorMessage = "Only jpg, jpeg and png files are allowed")]
    public required IFormFile File { get; set; }
}
