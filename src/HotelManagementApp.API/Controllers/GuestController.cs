using HotelManagementApp.Application.CQRS.Role.Add;
using HotelManagementApp.Application.CQRS.Role.GetAll;
using HotelManagementApp.Application.CQRS.Role.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
