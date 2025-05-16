using HotelManagementApp.Application.CQRS.Role.Add;
using HotelManagementApp.Application.CQRS.Role.GetAll;
using HotelManagementApp.Application.CQRS.Role.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/guest")]
[ApiController]
[Authorize(Roles = "Staff, Manager, Admin")]
public class GuestController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns all guests (staff and above).
    /// </summary>
    /// <response code="200">Returns list of all guest accounts</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to view guest list</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllGuests(CancellationToken ct)
    {
        var query = new GetAccountsInRoleQuery { RoleName = "Guest" };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Adds a user to the Guest role (staff and above).
    /// </summary>
    /// <response code="204">User added to Guest role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to modify roles</response>
    /// <response code="404">User not found</response>
    /// <response code="409">User is already in the Guest role</response>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddToRole(string userId, CancellationToken ct)
    {
        var cmd = new AddToRoleCommand
        {
            UserId = userId,
            Role = "Guest"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a user from the Guest role (staff and above).
    /// </summary>
    /// <response code="204">User removed from Guest role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to modify roles</response>
    /// <response code="404">User not found</response>
    /// <response code="409">User is not in the Guest role</response>
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
            Role = "Guest"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}