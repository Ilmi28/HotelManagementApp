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
    /// Adds the Staff role to a user. Only Manager or Admin can perform this.
    /// </summary>
    /// <param name="userId">User's ID to assign Staff role.</param>
    /// <response code="204">User successfully assigned to Staff role.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">User lacks permission.</response>
    /// <response code="404">User not found.</response>
    /// <response code="409">User already in Staff role.</response>
    [HttpPatch("add/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddToStaff(string userId, CancellationToken ct)
    {
        var cmd = new AddToRoleCommand { UserId = userId, Role = "Staff" };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Removes the Staff role from a user. Only Manager or Admin can perform this.
    /// </summary>
    /// <param name="userId">User's ID to remove Staff role.</param>
    /// <response code="204">User successfully removed from Staff role.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">User lacks permission.</response>
    /// <response code="404">User not found.</response>
    /// <response code="409">User not in Staff role.</response>
    [HttpPatch("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RemoveFromStaff(string userId, CancellationToken ct)
    {
        var cmd = new RemoveFromRoleCommand { UserId = userId, Role = "Staff" };
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Gets all users with the Staff role. Requires Manager or Admin role.
    /// </summary>
    /// <response code="200">Returns list of staff users.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="403">User lacks permission.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllStaff(CancellationToken ct)
    {
        var query = new GetAccountsInRoleQuery { RoleName = "Staff" };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
