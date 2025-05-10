using HotelManagementApp.Application.CQRS.Role.Remove;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Removes a user from the Admin role (admin).
    /// </summary>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromRole(string userId, CancellationToken ct)
    {
        var cmd = new RemoveFromRoleCommand
        {
            UserId = userId,
            Role = "Admin"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Adds a user to the Admin role (admin).
    /// </summary>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToRole(string userId, CancellationToken ct)
    {
        var cmd = new RemoveFromRoleCommand
        {
            UserId = userId,
            Role = "Admin"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}
