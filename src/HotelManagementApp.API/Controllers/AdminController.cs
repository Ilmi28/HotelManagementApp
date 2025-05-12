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
    /// Removes a user from the Admin role
    /// </summary>
    /// <response code="204">User removed from Admin role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is not authorized to remove Admin role</response>
    /// <response code="404">User not found</response>
    /// <response code="409">Cannot remove Admin role from this user</response>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    /// Adds a user to the Admin role
    /// </summary>
    /// <response code="204">User added to Admin role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is not authorized to add Admin role</response>
    /// <response code="404">User not found</response>
    /// <response code="409">Cannot add Admin role to this user</response>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
