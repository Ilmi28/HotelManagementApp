using HotelManagementApp.Application.CQRS.UserProfilePicture.GetProfilePicture;
using HotelManagementApp.Application.CQRS.UserProfilePicture.UpdateProfilePicture;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/profile-picture")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ProfilePictureController(IMediator mediator) : ControllerBase
{
    [HttpPost("upload/profile-picture/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file, string userId, CancellationToken ct)
    {
        var cmd = new UpdateProfilePictureCommand
        {
            File = file,
            UserId = userId
        };
        var fileName = await mediator.Send(cmd, ct);
        return Ok(fileName);
    }

    [HttpGet("profile-picture/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfilePicture(string userId, CancellationToken ct)
    {
        var profilePicture = await mediator.Send(new GetProfilePictureQuery { UserId = userId }, ct);
        return File(profilePicture, "image/jpeg");
    }
}
