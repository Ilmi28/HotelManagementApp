using HotelManagementApp.Application.CQRS.Role.Add;
using HotelManagementApp.Application.CQRS.Role.GetAll;
using HotelManagementApp.Application.CQRS.Role.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/staff")]
[ApiController]
[Authorize(Roles = "Manager, Admin")]
public class StaffController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a user to the Staff role (manager and above).
    /// </summary>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddToStaff(string userId, CancellationToken ct)
    {
        var cmd = new AddToRoleCommand
        {
            UserId = userId,
            Role = "Staff"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes a user from the Staff role (manager and above).
    /// </summary>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveFromStaff(string userId, CancellationToken ct)
    {
        var cmd = new RemoveFromRoleCommand
        {
            UserId = userId,
            Role = "Staff"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns all staff members (manager and above).
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllStaff(CancellationToken ct)
    {
        var query = new GetAccountsInRoleQuery
        {
            RoleName = "Staff"
        };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
