using HotelManagementApp.Application.CQRS.Role.Add;
using HotelManagementApp.Application.CQRS.Role.GetAll;
using HotelManagementApp.Application.CQRS.Role.Remove;
using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/manager")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ManagerController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Removes a user from the Manager role (admin only).
    /// </summary>
    /// <response code="204">User removed from role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to remove roles</response>
    /// <response code="404">User not found</response>
    /// <response code="409">User is not in the specified role</response>
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
            Role = "Manager"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Adds a user to the Manager role (admin only).
    /// </summary>
    /// <response code="204">User added to role successfully</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to add roles</response>
    /// <response code="404">User not found</response>
    /// <response code="409">User is already in the specified role</response>
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
            Role = "Manager"
        };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns all users in the Manager role (admin only).
    /// </summary>
    /// <response code="200">Returns list of managers</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="403">User is unauthorized to view role members</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllManagers(CancellationToken ct)
    {
        var query = new GetAccountsInRoleQuery
        {
            RoleName = "Manager"
        };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
